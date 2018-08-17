﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Soldier
{
    public class DataMgr
    {
        //单例
        private static DataMgr instance;
        public static DataMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataMgr();
                }
                return instance;
            }
        }
        public void Reset() {
            SoldierDic.Clear();
            DarkBuildDic.Clear();
            BrightBuildDic.Clear();
            SoldierId = 0;
            SoldierBuildId = 0;
        }
        //存放技能名称的字典
        public Dictionary<SkillEnum, string> SkillDic;
        //地图上存在的所有士兵的字典
        public Dictionary<int, SoldierCtrl> SoldierDic;
        //黑暗方建筑字典
        public Dictionary<int, BuildCtrl> DarkBuildDic;
        //光明方建筑字典
        public Dictionary<int, BuildCtrl> BrightBuildDic;
        //对象和它的子对象的名称对应
        public Dictionary<string, string> ObjChildObjDic;
        //士兵和建筑的id号
        public static int SoldierId = 0;
        public static int SoldierBuildId = 0;
        //key可以是枚举
        public Dictionary<string, SoldierInfo> dicInfo { get; private set; }
        //根据编号获取兵种名称
        public Dictionary<int, string> PrefebsDic { get; private set; }
        //根据攻击类型和护甲类型获取实际伤害比
        public Dictionary<AttackArmor, float> DamageRatio { get; private set; }
        private DataMgr()
        {
            //将所有士兵的属性信息存在字典里。
            dicInfo = new Dictionary<string, SoldierInfo>();
            //当前地图上存在的所有士兵
            SoldierDic = new Dictionary<int, SoldierCtrl>();
            //当前地图上存在的所有我的建筑
            DarkBuildDic = new Dictionary<int, BuildCtrl>();
            //当前地图上存在的所有敌方建筑
            BrightBuildDic = new Dictionary<int, BuildCtrl>();
            //技能字典
            SkillDic = new Dictionary<SkillEnum, string>();
            dicInfo.Add("Terren_Cavalry_1", AllInfo.Terren_Cavalry_1);
            dicInfo.Add("Terren_Cavalry_2", AllInfo.Terren_Cavalry_2);
            dicInfo.Add("Terren_Cavalry_3", AllInfo.Terren_Cavalry_3);

            dicInfo.Add("Terren_Spear_1", AllInfo.Terren_Spear_1);
            dicInfo.Add("Terren_Spear_2", AllInfo.Terren_Spear_2);
            dicInfo.Add("Terren_Spear_3", AllInfo.Terren_Spear_3);

            dicInfo.Add("Terren_Archer_1", AllInfo.Terren_Archer_1);
            dicInfo.Add("Terren_Archer_2", AllInfo.Terren_Archer_2);
            dicInfo.Add("Terren_Archer_3", AllInfo.Terren_Archer_3);

            dicInfo.Add("Terren_Infantry_1", AllInfo.Terren_Infantry_1);
            dicInfo.Add("Terren_Infantry_2", AllInfo.Terren_Infantry_2);
            dicInfo.Add("Terren_Infantry_3", AllInfo.Terren_Infantry_3);

            dicInfo.Add("Terren_Worker_1", AllInfo.Terren_Worker_1);
            dicInfo.Add("Terren_Worker_2", AllInfo.Terren_Worker_2);
            dicInfo.Add("Terren_Worker_3", AllInfo.Terren_Worker_3);

            dicInfo.Add("Orc_Swordsman_1", AllInfo.Orc_Swordsman_1);
            dicInfo.Add("Orc_Swordsman_2", AllInfo.Orc_Swordsman_2);
            dicInfo.Add("Orc_Swordsman_3", AllInfo.Orc_Swordsman_3);

            dicInfo.Add("Orc_Assassin_1", AllInfo.Orc_Assassin_1);
            dicInfo.Add("Orc_Assassin_2", AllInfo.Orc_Assassin_2);
            dicInfo.Add("Orc_Assassin_3", AllInfo.Orc_Assassin_3);

            dicInfo.Add("Orc_Magic_1", AllInfo.Orc_Magic_1);
            dicInfo.Add("Orc_Magic_2", AllInfo.Orc_Magic_2);
            dicInfo.Add("Orc_Magic_3", AllInfo.Orc_Magic_3);

            dicInfo.Add("Orc_Infantry_1", AllInfo.Orc_Infantry_1);
            dicInfo.Add("Orc_Infantry_2", AllInfo.Orc_Infantry_2);
            dicInfo.Add("Orc_Infantry_3", AllInfo.Orc_Infantry_3);

            dicInfo.Add("Orc_Scout_1", AllInfo.Orc_Scout_1);
            dicInfo.Add("Orc_Scout_2", AllInfo.Orc_Scout_2);
            dicInfo.Add("Orc_Scout_3", AllInfo.Orc_Scout_3);
            //Orc_Assassin = 1,    //兽族刺客
            //Orc_Magic = 2,       //兽族法师
            //Orc_Scout = 3,       //兽族投石手
            //Orc_Swordsman = 4,   //兽族剑士
            //Orc_Infantry = 5,    //兽族步兵
            //Terren_Worker = 6,   //人族工人
            //Terren_Spear = 7,    //人族长枪兵
            //Terren_Cavalry = 8,  //人族骑兵
            //Terren_Archer = 9,   //人族弓箭手
            //Terren_Infantry = 10 //人族步兵
            PrefebsDic = new Dictionary<int, string>();
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Orc_Assassin), "Orc_Assassin");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Orc_Magic), "Orc_Magic");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Orc_Scout), "Orc_Scout");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Orc_Swordsman), "Orc_Swordsman");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Orc_Infantry), "Orc_Infantry");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Terren_Worker), "Terren_Worker");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Terren_Spear), "Terren_Spear");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Terren_Cavalry), "Terren_Cavalry");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Terren_Archer), "Terren_Archer");
            PrefebsDic.Add(Convert.ToInt32(SoldierEnum.Terren_Infantry), "Terren_Infantry");
            //public enum AttackSpecies
            //{
            //    Normol,                     //普通攻击
            //    Spell,                      //魔法攻击
            //    Pierce                      //穿刺攻击
            //}
            //public enum ArmorSpecies
            //{
            //    Light,                      //轻甲
            //    Heavy,                      //重甲
            //    Leather                     //皮甲
            //}
            //穿透攻击造成伤害
            //皮甲  轻甲 重甲
            //120 % 110 % 80 %

            //魔法攻击造成伤害
            //皮甲 轻甲  重甲
            //100 % 110 % 120 %

            //普通攻击造成伤害
            //皮甲 轻甲  重甲
            //110 % 100 % 90 %
            DamageRatio = new Dictionary<AttackArmor, float>();
            DamageRatio.Add(new AttackArmor(AttackSpecies.Pierce, ArmorSpecies.Leather), 1.2f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Pierce, ArmorSpecies.Light), 1.1f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Pierce, ArmorSpecies.Heavy), 0.8f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Spell, ArmorSpecies.Leather), 1f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Spell, ArmorSpecies.Light), 1.1f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Spell, ArmorSpecies.Heavy), 1.2f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Normol, ArmorSpecies.Leather), 1.1f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Normol, ArmorSpecies.Light), 1f);
            DamageRatio.Add(new AttackArmor(AttackSpecies.Normol, ArmorSpecies.Heavy), 0.9f);

            ObjChildObjDic = new Dictionary<string, string>();
            ObjChildObjDic.Add("Orc_Assassin_Build(Clone)", "Orc_Assassin");
            ObjChildObjDic.Add("Orc_Assassin", "Orc_Assassin");
            ObjChildObjDic.Add("Orc_Infantry_Build(Clone)", "Orc_Warrior");
            ObjChildObjDic.Add("Orc_Infantry", "Orc_Warrior");
            ObjChildObjDic.Add("Orc_Magic_Build(Clone)", "Orc_Mystic");
            ObjChildObjDic.Add("Orc_Magic", "Orc_Mystic");
            ObjChildObjDic.Add("Orc_Scout_Build(Clone)", "Orc_Scout");
            ObjChildObjDic.Add("Orc_Scout", "Orc_Scout");
            ObjChildObjDic.Add("Orc_Swordsman_Build(Clone)", "Orc_Gladiator");
            ObjChildObjDic.Add("Orc_Swordsman", "Orc_Gladiator");
            ObjChildObjDic.Add("Terren_Archer_Build(Clone)", "WK_archer");
            ObjChildObjDic.Add("Terren_Archer", "WK_archer");
            ObjChildObjDic.Add("Terren_Cavalry_Build(Clone)", "WK_HeavyIntantry");
            ObjChildObjDic.Add("Terren_Cavalry", "WK_HeavyIntantry");
            ObjChildObjDic.Add("Terren_Infantry_Build(Clone)", "WK_HeavyIntantry");
            ObjChildObjDic.Add("Terren_Infantry", "WK_HeavyIntantry");
            ObjChildObjDic.Add("Terren_Spear_Build(Clone)", "WK_Spearman");
            ObjChildObjDic.Add("Terren_Spear", "WK_Spearman");
            ObjChildObjDic.Add("Terren_Worker_Build(Clone)", "WK_worker");
            ObjChildObjDic.Add("Terren_Worker", "WK_worker");
            //None,                //恢复默认
            //Stop,           //减速
            //SpeedUp             //加速
            SkillDic.Add(SkillEnum.Stop, "Stop");
            SkillDic.Add(SkillEnum.SpeedUp, "SpeedUp");
            SkillDic.Add(SkillEnum.None, "SpeedInfluenceCancel");
        }
    }
    //public int MAXHP { get; set; }          //血量上限
    //public int AttackPower { get; set; }    //攻击力    
    //public int Armor { get; set; }          //护甲值
    //public float Speed { get; set; }        //速度
    //public float AttackRange { get; set; }  //攻击范围
    //public int Level { get; set; }          //兵种等级（根据建筑等级）
    //public Race race { get; set; }          //种族
    //public AttackSpecies attackSpecies { get; set; }    //攻击类型
    //public ArmorSpecies armorSpecies { get; set; }      //护甲类型
    public class AllInfo
    {
        //把各种族各兵种所有的信息全初始化
        /****************************************************兽族信息***************************************************************/
        //人族骑兵
        public static SoldierInfo Terren_Cavalry_1 = new SoldierInfo { Name = "骑士(人族)", Cost = 20, MAXHP = 120, attackSpecies = AttackSpecies.Normol, AttackPower = 15, armorSpecies = ArmorSpecies.Light, Armor = 3, Speed = 2, AttackRange = 2, Reward = 5, race = Race.Terren };
        public static SoldierInfo Terren_Cavalry_2 = new SoldierInfo { Name = "骑士(人族)", Cost = 40, MAXHP = 240, attackSpecies = AttackSpecies.Normol, AttackPower = 25, armorSpecies = ArmorSpecies.Light, Armor = 6, Speed = 2, AttackRange = 2, Reward = 10, race = Race.Terren };
        public static SoldierInfo Terren_Cavalry_3 = new SoldierInfo { Name = "骑士(人族)", Cost = 80, MAXHP = 600, attackSpecies = AttackSpecies.Normol, AttackPower = 35, armorSpecies = ArmorSpecies.Light, Armor = 9, Speed = 2, AttackRange = 2, Reward = 20, race = Race.Terren };
        //人族长枪兵
        public static SoldierInfo Terren_Spear_1 = new SoldierInfo { Name = "长枪兵(人族)", Cost = 15, MAXHP = 80, attackSpecies = AttackSpecies.Pierce, AttackPower = 10, armorSpecies = ArmorSpecies.Heavy, Armor = 3, Speed = 2, AttackRange = 2, Reward = 4, race = Race.Terren };
        public static SoldierInfo Terren_Spear_2 = new SoldierInfo { Name = "长枪兵(人族)", Cost = 30, MAXHP = 160, attackSpecies = AttackSpecies.Pierce, AttackPower = 15, armorSpecies = ArmorSpecies.Heavy, Armor = 6, Speed = 2, AttackRange = 2, Reward = 8, race = Race.Terren };
        public static SoldierInfo Terren_Spear_3 = new SoldierInfo { Name = "长枪兵(人族)", Cost = 60, MAXHP = 340, attackSpecies = AttackSpecies.Pierce, AttackPower = 20, armorSpecies = ArmorSpecies.Heavy, Armor = 9, Speed = 2, AttackRange = 2, Reward = 16, race = Race.Terren };
        //人族弓箭手
        public static SoldierInfo Terren_Archer_1 = new SoldierInfo { Name = "弓箭手(人族)", Cost = 10, MAXHP = 60, attackSpecies = AttackSpecies.Pierce, AttackPower = 8, armorSpecies = ArmorSpecies.Light, Armor = 2, Speed = 2, AttackRange = 5, Reward = 3, race = Race.Terren };
        public static SoldierInfo Terren_Archer_2 = new SoldierInfo { Name = "弓箭手(人族)", Cost = 20, MAXHP = 120, attackSpecies = AttackSpecies.Pierce, AttackPower = 12, armorSpecies = ArmorSpecies.Light, Armor = 4, Speed = 2, AttackRange = 5, Reward = 6, race = Race.Terren };
        public static SoldierInfo Terren_Archer_3 = new SoldierInfo { Name = "弓箭手(人族)", Cost = 40, MAXHP = 250, attackSpecies = AttackSpecies.Pierce, AttackPower = 16, armorSpecies = ArmorSpecies.Light, Armor = 6, Speed = 2, AttackRange = 5, Reward = 12, race = Race.Terren };
        //人族步兵
        public static SoldierInfo Terren_Infantry_1 = new SoldierInfo { Name = "步兵(人族)", Cost = 10, MAXHP = 100, attackSpecies = AttackSpecies.Normol, AttackPower = 6, armorSpecies = ArmorSpecies.Heavy, Armor = 5, Speed = 2, AttackRange = 2, Reward = 3, race = Race.Terren };
        public static SoldierInfo Terren_Infantry_2 = new SoldierInfo { Name = "步兵(人族)", Cost = 20, MAXHP = 200, attackSpecies = AttackSpecies.Normol, AttackPower = 10, armorSpecies = ArmorSpecies.Heavy, Armor = 8, Speed = 2, AttackRange = 2, Reward = 6, race = Race.Terren };
        public static SoldierInfo Terren_Infantry_3 = new SoldierInfo { Name = "步兵(人族)", Cost = 40, MAXHP = 420, attackSpecies = AttackSpecies.Normol, AttackPower = 14, armorSpecies = ArmorSpecies.Heavy, Armor = 11, Speed = 2, AttackRange = 2, Reward = 12, race = Race.Terren };
        //工人
        public static SoldierInfo Terren_Worker_1 = new SoldierInfo { Name = "工人(人族)", Cost = 5, MAXHP = 40, attackSpecies = AttackSpecies.Normol, AttackPower = 4, armorSpecies = ArmorSpecies.Leather, Armor = 2, Speed = 2, AttackRange = 2, Reward = 2, race = Race.Terren };
        public static SoldierInfo Terren_Worker_2 = new SoldierInfo { Name = "工人(人族)", Cost = 10, MAXHP = 80, attackSpecies = AttackSpecies.Normol, AttackPower = 8, armorSpecies = ArmorSpecies.Leather, Armor = 4, Speed = 2, AttackRange = 2, Reward = 4, race = Race.Terren };
        public static SoldierInfo Terren_Worker_3 = new SoldierInfo { Name = "工人(人族)", Cost = 20, MAXHP = 180, attackSpecies = AttackSpecies.Normol, AttackPower = 12, armorSpecies = ArmorSpecies.Leather, Armor = 6, Speed = 2, AttackRange = 2, Reward = 8, race = Race.Terren };
        /****************************************************兽族信息***************************************************************/
        //兽族剑士
        public static SoldierInfo Orc_Swordsman_1 = new SoldierInfo { Name = "剑士(兽族)", Cost = 25, MAXHP = 160, attackSpecies = AttackSpecies.Normol, AttackPower = 15, armorSpecies = ArmorSpecies.Light, Armor = 3, Speed = 2, AttackRange = 2, Reward = 5, race = Race.Orc };
        public static SoldierInfo Orc_Swordsman_2 = new SoldierInfo { Name = "剑士(兽族)", Cost = 50, MAXHP = 400, attackSpecies = AttackSpecies.Normol, AttackPower = 20, armorSpecies = ArmorSpecies.Light, Armor = 6, Speed = 2, AttackRange = 2, Reward = 10, race = Race.Orc };
        public static SoldierInfo Orc_Swordsman_3 = new SoldierInfo { Name = "剑士(兽族)", Cost = 100,MAXHP = 900, attackSpecies = AttackSpecies.Normol, AttackPower = 25, armorSpecies = ArmorSpecies.Light, Armor = 9, Speed = 2, AttackRange = 2, Reward = 20, race = Race.Orc };
        //兽族刺客
        public static SoldierInfo Orc_Assassin_1 = new SoldierInfo { Name = "刺客(兽族)", Cost = 20, MAXHP = 80, attackSpecies = AttackSpecies.Pierce,  AttackPower = 20, armorSpecies = ArmorSpecies.Leather, Armor = 2, Speed = 2, AttackRange = 2, Reward = 4, race = Race.Orc };
        public static SoldierInfo Orc_Assassin_2 = new SoldierInfo { Name = "刺客(兽族)", Cost = 40, MAXHP = 160, attackSpecies = AttackSpecies.Pierce, AttackPower = 30, armorSpecies = ArmorSpecies.Leather, Armor = 4, Speed = 2, AttackRange = 2, Reward = 8, race = Race.Orc };
        public static SoldierInfo Orc_Assassin_3 = new SoldierInfo { Name = "刺客(兽族)", Cost = 80, MAXHP = 340, attackSpecies = AttackSpecies.Pierce, AttackPower = 40, armorSpecies = ArmorSpecies.Leather, Armor = 6, Speed = 2, AttackRange = 2, Reward = 16, race = Race.Orc };
        //兽族法师
        public static SoldierInfo Orc_Magic_1 = new SoldierInfo { Name = "法师(兽族)", Cost = 12, MAXHP = 80,  attackSpecies = AttackSpecies.Spell, AttackPower = 8,  armorSpecies = ArmorSpecies.Leather, Armor = 2, Speed = 2, AttackRange = 5, Reward = 3, race = Race.Orc };
        public static SoldierInfo Orc_Magic_2 = new SoldierInfo { Name = "法师(兽族)", Cost = 24, MAXHP = 160, attackSpecies = AttackSpecies.Spell, AttackPower = 12, armorSpecies = ArmorSpecies.Leather, Armor = 4, Speed = 2, AttackRange = 5, Reward = 6, race = Race.Orc };
        public static SoldierInfo Orc_Magic_3 = new SoldierInfo { Name = "法师(兽族)", Cost = 36, MAXHP = 340, attackSpecies = AttackSpecies.Spell, AttackPower = 16, armorSpecies = ArmorSpecies.Leather, Armor = 6, Speed = 2, AttackRange = 5, Reward = 12, race = Race.Orc };
        //兽族步兵
        public static SoldierInfo Orc_Infantry_1 = new SoldierInfo { Name = "步兵(兽族)", Cost = 12, MAXHP = 120, attackSpecies = AttackSpecies.Normol, AttackPower = 8, armorSpecies = ArmorSpecies.Light, Armor = 4, Speed = 2, AttackRange = 2, Reward = 3, race = Race.Orc };
        public static SoldierInfo Orc_Infantry_2 = new SoldierInfo { Name = "步兵(兽族)", Cost = 24, MAXHP = 240, attackSpecies = AttackSpecies.Normol, AttackPower = 12, armorSpecies = ArmorSpecies.Light, Armor = 6, Speed = 2, AttackRange = 2, Reward = 6, race = Race.Orc };
        public static SoldierInfo Orc_Infantry_3 = new SoldierInfo { Name = "步兵(兽族)", Cost = 48, MAXHP = 500, attackSpecies = AttackSpecies.Normol, AttackPower = 16, armorSpecies = ArmorSpecies.Light, Armor = 10, Speed = 2, AttackRange = 2, Reward = 12, race = Race.Orc };
        //兽族投石手
        public static SoldierInfo Orc_Scout_1 = new SoldierInfo { Name = "投石手(兽族)", Cost = 8, MAXHP = 60,  attackSpecies = AttackSpecies.Normol, AttackPower = 5, armorSpecies = ArmorSpecies.Leather, Armor = 2, Speed = 2, AttackRange = 5, Reward = 2, race = Race.Orc };
        public static SoldierInfo Orc_Scout_2 = new SoldierInfo { Name = "投石手(兽族)", Cost = 16, MAXHP = 120, attackSpecies = AttackSpecies.Normol, AttackPower = 10, armorSpecies = ArmorSpecies.Leather, Armor = 4, Speed = 2, AttackRange = 5, Reward = 4, race = Race.Orc };
        public static SoldierInfo Orc_Scout_3 = new SoldierInfo { Name = "投石手(兽族)", Cost = 32, MAXHP = 260, attackSpecies = AttackSpecies.Normol, AttackPower = 15, armorSpecies = ArmorSpecies.Leather, Armor = 6, Speed = 2, AttackRange = 5, Reward = 8, race = Race.Orc };
    }
    
    public struct AttackArmor               //攻击力和护甲之间的关系
    {
        AttackSpecies attackSpecies;
        ArmorSpecies armorSpecies;

        public AttackArmor(AttackSpecies attackSpecies, ArmorSpecies armorSpecies)
        {
            this.attackSpecies = attackSpecies;
            this.armorSpecies = armorSpecies;
        }
    }
}


/*
护甲减少伤害百分比=（护甲值*0.06）/（护甲值*0.06+1）



造成伤害=攻击力*类型百分比*（1-护甲减少伤害百分比）
 */