using System;
using System.Collections;
using System.Collections.Generic;
using War_Server.Stats;
using Common;
namespace War_Server.Servers
{
    public class PlayerInfo
    {
        public List<BaseStat> playerStats = new List<BaseStat>();

        public PlayerInfo(int maxHelath,int health,int moveSpeed,int AttackSpeed)
        {
            playerStats = new List<BaseStat>()
            {
                new BaseStat(BaseStatType.MaxHealth,maxHelath,"MaxHealth"),
                new BaseStat(BaseStatType.Health,health,"Health"),
                new BaseStat(BaseStatType.AttackSpeed, AttackSpeed, "AttackSpeed"),
                new BaseStat(BaseStatType.MoveSpeed,moveSpeed,"MoveSpeed"),
                new BaseStat(BaseStatType.SpeedPoint,0,"RompagePoint")
            };
        }

        public BaseStat GetStat(BaseStatType statType)
        {
            return this.playerStats.Find(x => x.StatType == statType);
        }

        public void AddValueByItem(List<BaseStat> baseStats)
        {
            foreach(BaseStat s in baseStats)
            {
                GetStat(s.StatType).AddExtraValue(new StatBonus(s.BaseValue));
            }
        }

        public void AddValue(BaseStatType statType,int value)
        {
            GetStat(statType).AddExtraValue(new StatBonus(value));
        }

        public void RemoveValueByItem(List<BaseStat> baseStats)
        {
            foreach(BaseStat s in baseStats)
            {
                GetStat(s.StatType).RemoveExtraValue(new StatBonus(s.BaseValue));
            }
        }

        public void RemoveValue(BaseStatType statType,int value)
        {
            GetStat(statType).RemoveExtraValue(new StatBonus(value));
        }
    }
}
