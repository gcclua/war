﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using System.Reflection;


namespace Assets.Scripts.Soldier
{
    public class PlayerCtrl : MonoBehaviour
    {

        public int PlayerID { get; private set; }       //玩家ID
        public static Camp Camp { get; private set; }          //阵营
        private bool SelectBuild;                       //是否选定需要建造的建筑物
        private Vector3 BuildPos;                       //建造建筑的位置
        private Quaternion StartRotate;                 //旋转预制体使其朝向正确的方向
        private Vector3 CorrectMove;                    //修正在选定砖上的距离
        private String BuildArea;                       //可以建造建筑的区域
        private string soldierName;                     //士兵名称
        private Animator ani;
        private GameObject newBuild;                            //当前创建的建筑
        private int LightBuildID;                       //当前亮着的建筑物的ID
        private object lockObject = new object();                     //对象锁

        //创建建筑声音特效
        private AudioClip createBuilding;
        //点击建筑之后的音效
        private AudioClip Orc_Assassin;
        private AudioClip Orc_Infantry;
        private AudioClip Orc_Magic;
        private AudioClip Orc_Scout;
        private AudioClip Orc_Swordsman;
        private AudioClip Terren_Cavalry;
        private AudioClip Terren_Spear;
        private AudioClip Terren_Infantry;
        private AudioClip Terren_Worker;
        private AudioClip Terren_Archer;
        private AudioSource audio;

        
        public void Init(int playerId, Camp camp)
        {
            PlayerID = playerId;
            Camp = camp;
            Correct();
        }
        // Use this for initialization
        void Start()
        {
            audio=gameObject.AddComponent<AudioSource>();
            audio.volume = 0.5f;
            NetDispacher.Instance.AddEventListener("sendBuildPosition_bcst", OnSendBuildPosition_bcst);
            NetDispacher.Instance.AddEventListener("sendStartSoldier_bcst", OnsendStartSoldier_bcst);
            NetDispacher.Instance.AddEventListener("sendSoldierPos_bcst", OnsendSoldierPos_bcst);
            NetDispacher.Instance.AddEventListener("sendBuileLevelUp_bcst", OnSendBuileLevelUp_bcst);
            NetDispacher.Instance.AddEventListener("soldierBreak_bcst", OnSoldierBreak_bcst);
            
            

            LightBuildID = -1;
            ani = GetComponent<Animator>();
            InvokeRepeating("SendSoldierPosition", 0, 1f / 30f);
            Orc_Assassin=(AudioClip)Resources.Load("Music/Orc_Assassin", typeof(AudioClip));
            Orc_Infantry = (AudioClip)Resources.Load("Music/Orc_Infantry", typeof(AudioClip));
            Orc_Magic = (AudioClip)Resources.Load("Music/Orc_Magic", typeof(AudioClip));
            Orc_Scout = (AudioClip)Resources.Load("Music/Orc_Scout", typeof(AudioClip));
            Orc_Swordsman = (AudioClip)Resources.Load("Music/Orc_Swordsman", typeof(AudioClip));
            Terren_Cavalry = (AudioClip)Resources.Load("Music/Terren_Cavalry", typeof(AudioClip));
            Terren_Spear = (AudioClip)Resources.Load("Music/Terren_Spear", typeof(AudioClip));
            Terren_Infantry = (AudioClip)Resources.Load("Music/Terren_Infantry", typeof(AudioClip));
            Terren_Worker = (AudioClip)Resources.Load("Music/Terren_Worker", typeof(AudioClip));
            Terren_Archer = (AudioClip)Resources.Load("Music/Terren_Archer", typeof(AudioClip));
            createBuilding = (AudioClip)Resources.Load("Music/createBuilding", typeof(AudioClip));

        }
        //没帧更新
        void Update()
        {
            /****************************技能*******************************/
            ///实时根据技能标记执行相应操作
            UseSkill();
            //点击左键(非按钮)
            if (Input.GetMouseButtonDown(0))
            {
                /****************************选中了卡片,建造******************************/
                if (LocalUser.Instance.CardStatus != SoldierEnum.None)
                {
                    BuildingConstruction();
                }
                /***************************未选中卡牌******************************/
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] array = Physics.RaycastAll(ray, Mathf.Infinity, 1 << (LayerMask.NameToLayer("Build")));
                    //如果选中了建筑物
                    if (array.Length > 0)
                    {
                        if (PlayerID == array[0].collider.gameObject.GetComponent<BuildCtrl>().PlayerID)
                        {
                            //关粒子特效
                            TurnDownLastBuildParticle();
                            //选中建筑音效
                            BuildCtrl buildCtrl = array[0].collider.gameObject.GetComponent<BuildCtrl>();
                            SelectBuildSound(buildCtrl);
                            //打开选中建筑的粒子特效
                            GameObject particle = array[0].collider.gameObject.transform.Find("MagicZoneGreen(Clone)").gameObject;

                            particle.SetActive(true);
                            LightBuildID = array[0].collider.gameObject.GetComponent<BuildCtrl>().BuildID;
                            LocalUser.Instance.SelectBuildId = array[0].collider.gameObject.GetComponent<BuildCtrl>().BuildID;
                        }

                    }
                    else
                    {
                        //关粒子特效
                        TurnDownLastBuildParticle();
                    }
                }
            }
        }


        private void OnDestroy()
        {
            NetDispacher.Instance.RemoveEventListener("sendBuildPosition_bcst", OnSendBuildPosition_bcst);
            NetDispacher.Instance.RemoveEventListener("sendStartSoldier_bcst", OnsendStartSoldier_bcst);
            NetDispacher.Instance.RemoveEventListener("sendSoldierPos_bcst", OnsendSoldierPos_bcst);
            NetDispacher.Instance.RemoveEventListener("sendBuileLevelUp_bcst", OnSendBuileLevelUp_bcst);
            NetDispacher.Instance.RemoveEventListener("soldierBreak_bcst", OnSoldierBreak_bcst);
        }

        /// <summary>
        /// 接收服务器同步其他玩家建筑等级的消息
        /// </summary>
        /// <param name="param"></param>
        private void OnSendBuileLevelUp_bcst(object msg)
        {
            var handle = msg as mmopb.sendBuileLevelUp_bcst;
            if (handle.roleId == LocalUser.Instance.PlayerId)
                return;
            GameObject enemyBuild = null;
            if (Camp == Camp.Bright)
            {
                if (DataMgr.Instance.DarkBuildDic.ContainsKey(handle.buildId))
                {
                    DataMgr.Instance.DarkBuildDic[handle.buildId].UpdateLevel(handle.level);
                    enemyBuild = DataMgr.Instance.DarkBuildDic[handle.buildId].gameObject;
                }
                
            }
            else
            {
                if (DataMgr.Instance.BrightBuildDic.ContainsKey(handle.buildId))
                {
                    DataMgr.Instance.BrightBuildDic[handle.buildId].UpdateLevel(handle.level);
                    enemyBuild = DataMgr.Instance.BrightBuildDic[handle.buildId].gameObject;
                }
            }
            
            if(enemyBuild != null)
            {
                Material material = Resources.Load("Materials/level" + handle.level) as Material;
                GameObject child = enemyBuild.transform.Find(DataMgr.Instance.ObjChildObjDic[enemyBuild.name]).gameObject;
                child.GetComponent<SkinnedMeshRenderer>().material = material;
            }
        }

        /// <summary>
        /// 接收服务器同步其他玩家小兵位置的消息
        /// </summary>
        /// <param name="param"></param>
        private void OnsendSoldierPos_bcst(object msg)
        {
            var handle = msg as mmopb.sendSoldierPos_bcst;
            if (handle.roleId == LocalUser.Instance.PlayerId)
                return;
            var dic=handle.soldierPosInfoList;
            foreach (var pair in dic)
            {
                if (DataMgr.Instance.SoldierDic.ContainsKey(pair.Key))
                {
                    DataMgr.Instance.SoldierDic[pair.Key].UpdatePos(pair.Value);
                }
            }
        }

        /// <summary>
        /// 每1/45秒向服务器发送己方小兵位置
        /// </summary>
        private void SendSoldierPosition()
        {
            var handle = new mmopb.sendSoldierPos_req();
            var dic = new Dictionary<int, mmopb.p_SoldierPosInfo>();
            foreach (var pair in DataMgr.Instance.SoldierDic)
            {
                if (pair.Value.playerId == LocalUser.Instance.PlayerId)
                {
                    SoldierCtrl soldier = pair.Value;
                    var info=new mmopb.p_SoldierPosInfo();
                    info.x = soldier.transform.position.x;
                    info.y = soldier.transform.position.y;
                    info.z = soldier.transform.position.z;
                    info.xQuternion = soldier.transform.rotation.x;
                    info.yQuternion = soldier.transform.rotation.y;
                    info.zQuternion = soldier.transform.rotation.z;
                    info.wQuternion = soldier.transform.rotation.w;
                    dic.Add(soldier.id, info);
                }
            }
            handle.soldierPosInfoList = dic;
            ClientNet.Instance.Send(ProtoBuf.ProtoHelper.EncodeWithName(handle));
        }

        /// <summary>
        /// 收到服务器消息，同步其他玩家建筑位置
        /// </summary>
        /// <param name="param"></param>
        private void OnSendBuildPosition_bcst(object param)
        {
            var handle = param as mmopb.sendBuildPosition_bcst;
            string enemyName = DataMgr.Instance.PrefebsDic[handle.buildType];
            if (handle.roleId == PlayerID) return;
            newBuild = (GameObject)Resources.Load("Prefabs/" + enemyName + "_Build");
            Quaternion quaternion = new Quaternion(handle.xQuternion, handle.yQuternion, handle.zQuternion,handle.wQuternion);
            BuildCtrl newBuildCtrl = Instantiate(newBuild, new Vector3(handle.x, handle.y, handle.z), quaternion).AddComponent<BuildCtrl>();
            GameObject particle = Resources.Load("Prefabs/MagicZoneGreen") as GameObject;
            GameObject newParticle = Instantiate(particle, newBuildCtrl.gameObject.transform);
            newParticle.SetActive(false);
            lock (lockObject)
            {

                if(Camp == Camp.Dark)
                {
                    DataMgr.Instance.BrightBuildDic.Add(DataMgr.SoldierBuildId, newBuildCtrl);
                }
                else
                {
                    DataMgr.Instance.DarkBuildDic.Add(DataMgr.SoldierBuildId, newBuildCtrl);
                }
                newBuildCtrl.Init(DataMgr.SoldierBuildId++, handle.roleId, Camp == Camp.Dark ? Camp.Bright : Camp.Dark, quaternion, enemyName);
            }
                     
        }
        
        
        /// <summary>
        /// 使用技能
        /// </summary>
        public void UseSkill()
        {
            //Debug.Log("asdadad11111111111111");
            Type type = typeof(SoldierCtrl);
            SkillEnum skillStatus = (Camp == Camp.Dark) ? LocalUser.Instance.DarkSkillStatus : LocalUser.Instance.BrightSkillStatus;        //根据阵营选择阵营的技能状态
            //Debug.Log("**************" + skillStatus + "**************");
            MethodInfo mt = type.GetMethod(DataMgr.Instance.SkillDic[skillStatus]);     //根据技能字典获取函数名并调用
            if (mt != null)
            {
                foreach (var e in DataMgr.Instance.SoldierDic)
                {
                    if (e.Value.playerId == PlayerID)
                    {
                        mt.Invoke(e.Value, null);
                    }
                }
            }
        }
        /// <summary>
        /// 建造建筑
        /// </summary>
        public void BuildingConstruction()
        {
            string tempBuildName = DataMgr.Instance.PrefebsDic[Convert.ToInt32(LocalUser.Instance.CardStatus)] + "_1";
            if (DataMgr.Instance.dicInfo[tempBuildName].Cost <= LocalUser.Instance.Energy)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                    //射线
                RaycastHit[] array = Physics.RaycastAll(ray, Mathf.Infinity, 1 << (LayerMask.NameToLayer(BuildArea)));      //BuildArea是自己的建造区域层
                if (array.Length > 0)
                {
                    BuildPos = array[0].transform.position;
                    array[0].collider.gameObject.layer = LayerMask.NameToLayer("haveBuildBrick");            //更改这块砖的层级，就不会重复建造了
                    BuildPos += CorrectMove;
                    BuildPos.y = (float)0.4;        //校准
                    InitBuild();
                    //创建建筑，发送消息给服务器。(可以写成一个方法)
                    var handle = new mmopb.sendBuildPosition_req();
                    //将枚举类型强转为int型
                    handle.buildType = Convert.ToInt32(LocalUser.Instance.CardStatus);
                    handle.x = BuildPos.x;
                    handle.y = BuildPos.y;
                    handle.z = BuildPos.z;
                    handle.xQuternion = StartRotate.x;
                    handle.yQuternion = StartRotate.y;
                    handle.zQuternion = StartRotate.z;
                    handle.wQuternion = StartRotate.w;

                    //将消息发送给服务器。
                    ClientNet.Instance.Send(ProtoBuf.ProtoHelper.EncodeWithName(handle));
                    LocalUser.Instance.CardStatus = SoldierEnum.None;
                    CardCtrl.instance.ResetCardColor();

                    /****************播放音效**************/
                    audio.clip = createBuilding;
                    audio.Play();
                }
                else
                {
                    CardCtrl.instance.ResetCardColor();
                }
            }
            else
            {
                CardCtrl.instance.ResetCardColor();
            }
        }
        /// <summary>
        /// 关闭上次选中的建筑的粒子特效
        /// </summary>
        public void TurnDownLastBuildParticle()
        {
            if (LightBuildID != -1)         //前一个打开特效的建筑的ID，关闭它
            {
                if (Camp == Camp.Bright)
                {
                    GameObject childParticle = DataMgr.Instance.BrightBuildDic[LightBuildID].transform.Find("MagicZoneGreen(Clone)").gameObject;
                    childParticle.SetActive(false);
                }
                else
                {
                    GameObject childParticle = DataMgr.Instance.DarkBuildDic[LightBuildID].transform.Find("MagicZoneGreen(Clone)").gameObject;
                    childParticle.SetActive(false);
                }

                LightBuildID = -1;
            }
        }
        /// <summary>
        /// 根据建筑物名称选择相应的配乐
        /// </summary>
        /// <param name="buildCtrl"></param>
        public void SelectBuildSound(BuildCtrl buildCtrl)
        {
            switch (buildCtrl.SoldierName)
            {
                case "Orc_Assassin":
                    audio.clip = Orc_Assassin;
                    audio.Play();
                    break;
                case "Orc_Infantry":
                    audio.clip = Orc_Infantry;
                    audio.Play();
                    break;
                case "Orc_Magic":
                    audio.clip = Orc_Magic;
                    audio.Play();
                    break;
                case "Orc_Scout":
                    audio.clip = Orc_Scout;
                    audio.Play();
                    break;
                case "Orc_Swordsman":
                    audio.clip = Orc_Swordsman;
                    audio.Play();
                    break;
                case "Terren_Cavalry":
                    audio.clip = Terren_Cavalry;
                    audio.Play();
                    break;
                case "Terren_Spear":
                    audio.clip = Terren_Spear;
                    audio.Play();
                    break;
                case "Terren_Infantry":
                    audio.clip = Terren_Infantry;
                    audio.Play();
                    break;
                case "Terren_Worker":
                    audio.clip = Terren_Worker;
                    audio.Play();
                    break;
                case "Terren_Archer":
                    audio.clip = Terren_Archer;
                    audio.Play();
                    break;
                default:
                    break;
            }
        }
        //初始化建筑
        void InitBuild()
        {
            //加载模型和材质贴图的预制体
            soldierName = DataMgr.Instance.PrefebsDic[(int)LocalUser.Instance.CardStatus];
            newBuild = (GameObject)Resources.Load("Prefabs/" + soldierName + "_Build");
            BuildCtrl newBuildCtrl = Instantiate(newBuild, BuildPos, StartRotate).AddComponent<BuildCtrl>();
            GameObject particle = Resources.Load("Prefabs/MagicZoneGreen") as GameObject;
            GameObject newParticle = Instantiate(particle, newBuildCtrl.gameObject.transform);
            newParticle.SetActive(false);
            lock (lockObject)
            {
                if(Camp == Camp.Bright)
                {
                    DataMgr.Instance.BrightBuildDic.Add(DataMgr.SoldierBuildId, newBuildCtrl);
                }
                else
                {
                    DataMgr.Instance.DarkBuildDic.Add(DataMgr.SoldierBuildId, newBuildCtrl);
                }
                newBuildCtrl.Init(DataMgr.SoldierBuildId++, PlayerID, Camp, StartRotate, soldierName);
            }
        }
        //施放技能


        //矫正兵的朝向及位置
        void Correct()
        {
            if (Camp == Camp.Bright)
            {
                StartRotate = Quaternion.Inverse(new Quaternion(0, (float)0.7, 0, (float)0.7));
                CorrectMove.x = (float)1;
                CorrectMove.z = (float)-0.5;
                BuildArea = "BrightBrick";
            }
            else
            {
                StartRotate = new Quaternion(0, (float)0.7, 0, (float)0.7);
                CorrectMove.x = (float)-1;
                CorrectMove.z = (float)1.5;
                BuildArea = "DarkBrick";
            }
        }
        /// <summary>
        ///接收服务器发兵消息
        /// </summary>
        /// <param name="param"></param>
        private void OnsendStartSoldier_bcst(object msg)
        {
            var handle = msg as mmopb.sendStartSoldier_bcst;
            LocalUser.Instance.Energy = handle.energy;
            PlayerInfoCtrl.Instance.ShowInfo();
            foreach (var pair in DataMgr.Instance.BrightBuildDic)
            {
                if(pair.Value!=null)
                pair.Value.StartSoldierEvery30S();
            }
            foreach (var pair in DataMgr.Instance.DarkBuildDic)
            {
                if (pair.Value != null)
                pair.Value.StartSoldierEvery30S();
            }
        }

        /// <summary>
        /// 接收服务器同步血量能量消息
        /// </summary>
        /// <param name="msg"></param>
        private void OnSoldierBreak_bcst(object msg)
        {
            var handle = msg as mmopb.soldierBreak_bcst;
            if (handle.infoList.Count > 1)
            {
                foreach (var info in handle.infoList)
                {
                    if (info.Key == LocalUser.Instance.PlayerId)
                    {
                        LocalUser.Instance.MyHp = info.Value.hp;
                        LocalUser.Instance.Energy = info.Value.energy;
                    }
                    else
                    {
                        LocalUser.Instance.EnemyHp = info.Value.hp;
                    }
                }
            }
            else
            {
                foreach (var info in handle.infoList)
                {
                    LocalUser.Instance.MyHp = info.Value.hp;
                    LocalUser.Instance.Energy = info.Value.energy;
                }
            }
            PlayerInfoCtrl.Instance.ShowInfo();
        }

    }
}
