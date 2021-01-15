using UnityEngine;

static class RecordFactory {
    public static StateRecord Get(ISavable o)
    {
        System.Type t = o.GetType();

        if (t == typeof(PlayerCollecting))
        {
            return new PlayerCollectingRecord();
        }

        else if (t == typeof(CoinCollectible))
        {
            return new CoinRecord();
        }

        else if (t == typeof(ItemCollectible))
        {
            return new ItemCollectibleRecord();
        }

        else if (t == typeof(ItemBox))
        {
            return new ItemBoxRecord();
        }
        
        else if (t == typeof(KeyBox))
        {
            return new KeyBoxRecord();
        }

        else if (t == typeof(Switch) || t == typeof(Lever) || t == typeof(CostLever))
        {
            return new ButtonRecord();
        }

        else if (t == typeof(QuestDialogue))
        {
            return new QuestRecord();
        }

        else if (t == typeof(UmberJackDialogue))
        {
            return new UmberJackDialogueRecord();
        }

        else if (t == typeof(InfoDisplayTrigger))
        {
            return new InfoDisplayTriggerRecord();
        }

        else if (t == typeof(CutsceneTrigger))
        {
            return new CutsceneTriggerRecord();
        }

        // WIP

        else
        {
            Debug.LogError("RecordFactory: No record type corresponds to object of type " + t);
            return null;
        }
    }
}
