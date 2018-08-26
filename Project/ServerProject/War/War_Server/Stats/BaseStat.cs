using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Common;
namespace War_Server.Stats
{
    public class BaseStat
    {
        public List<StatBonus> BaseAdditives { get; set; }
        public BaseStatType StatType{ get; set; }
        public int BaseValue{ get; set; }
        public string StatName{ get; set; }
        public string StatDescription { get; set; }
        public int FinalValue{ get; set; }

        public BaseStat(int baseValue,string statName,string statDescription)
        {
            this.BaseAdditives = new List<StatBonus>();
            this.BaseValue = baseValue;
            this.StatName = statName;
            this.StatDescription = statDescription;
        }

        public BaseStat(BaseStatType baseStatType,int basevalue,string statName)
        {
            this.BaseAdditives = new List<StatBonus>();
            this.StatType = baseStatType;
            this.BaseValue = basevalue;
            this.StatName = statName;
        }

        public void AddExtraValue(StatBonus statBonus)
        {
            this.BaseAdditives.Add(statBonus);
        }

        public void RemoveExtraValue(StatBonus statBonus)
        {
            this.BaseAdditives.Remove(statBonus);
        }

        public int GetFinalValue()
        {
            this.FinalValue = 0;
            this.BaseAdditives.ForEach(x => this.FinalValue += x.Value);
            this.FinalValue += this.BaseValue;
            return this.FinalValue;
        }

        public void ClearAdditive()
        {
            this.BaseAdditives.Clear();
        }

        public void SubBaseValue(int value)
        {
            this.BaseValue -= value;
        }

        public void AddBaseValue(int value)
        {
            this.BaseValue += value;
        }

        public void ChangeBaseValue(int value)
        {
            this.BaseValue = value;
        }

    }
}
