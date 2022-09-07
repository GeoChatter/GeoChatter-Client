using GeoChatter.Core.Storage;
using GeoChatter.Integrations;
using GeoChatter.Model.Enums;
using GeoChatter.Model;
using GeoChatter.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {

        private static void TriggerGameEndActions()
        {
            if (Settings.Default.GameEndAction && !string.IsNullOrEmpty(Settings.Default.GameEndActionID) && Settings.Default.GameEndActionName != "[none]")
            {
                StreamerbotClient.ExecuteAction(Settings.Default.GameEndActionID, Settings.Default.GameEndActionName);
            }
            if (Settings.Default.ObsGameEndExecute && !string.IsNullOrEmpty(Settings.Default.ObsGameEndAction) && !string.IsNullOrEmpty(Settings.Default.ObsGameEndSource))
            {
                obsClient.ModifySource(Settings.Default.ObsGameEndScene, Settings.Default.ObsGameEndSource, Settings.Default.ObsGameEndAction);
            }
        }
        private static void TriggerRoundEndActions()
        {
            if (Settings.Default.RoundEndAction && !string.IsNullOrEmpty(Settings.Default.RoundEndActionID) && Settings.Default.RoundEndActionName != "[none]")
            {
                StreamerbotClient.ExecuteAction(Settings.Default.RoundEndActionID, Settings.Default.RoundEndActionName);
            }
            if (Settings.Default.ObsRoundEndExecute && !string.IsNullOrEmpty(Settings.Default.ObsRoundEndAction) && !string.IsNullOrEmpty(Settings.Default.ObsRoundEndSource))
            {
                obsClient.ModifySource(Settings.Default.ObsRoundEndScene, Settings.Default.ObsRoundEndSource, Settings.Default.ObsRoundEndAction);
            }
        }
        private static void TriggerRoundStartActions()
        {
            if (Settings.Default.RoundStartAction && !string.IsNullOrEmpty(Settings.Default.RoundStartActionID) && Settings.Default.RoundStartActionName != "[none]")
            {
                StreamerbotClient.ExecuteAction(Settings.Default.RoundStartActionID, Settings.Default.RoundStartActionName);
            }
            if (Settings.Default.ObsRoundStartExecute && !string.IsNullOrEmpty(Settings.Default.ObsRoundStartAction) && !string.IsNullOrEmpty(Settings.Default.ObsRoundStartSource))
            {
                obsClient.ModifySource(Settings.Default.ObsRoundStartScene, Settings.Default.ObsRoundStartSource, Settings.Default.ObsRoundStartAction);
            }
        }


        private void TriggerSpecialScoreActions(Player player, double score)
        {
            double from = Settings.Default.SpecialScoreFrom;
            double to = Settings.Default.SpecialScoreTo;
            bool specialScoreHit = (to == 0) ? score == from && from > -1 : from <= score && score <= to;

            if (Settings.Default.SpecialScoreLabel && specialScoreHit)
            {
                LabelStorage.WriteLabel(LabelType.SpecialScore, player.FullDisplayName, LabelPath);
            }

            if (Settings.Default.SpecialScoreAction && !string.IsNullOrEmpty(Settings.Default.SpecialScoreActionID) && Settings.Default.SpecialScoreActionName != "[none]" && specialScoreHit)
            {
                StreamerbotClient.ExecuteAction(Settings.Default.SpecialScoreActionID, Settings.Default.SpecialScoreActionName);
            }
            if (Settings.Default.ObsSpecialScoreExecute && !string.IsNullOrEmpty(Settings.Default.ObsSpecialScoreSource) && !string.IsNullOrEmpty(Settings.Default.ObsSpecialScoreAction) && specialScoreHit)
            {
                obsClient.ModifySource(Settings.Default.ObsSpecialScoreScene, Settings.Default.ObsSpecialScoreSource, Settings.Default.ObsSpecialScoreAction);
            }
        }

        private void TriggerSpecialDistanceActions(Player player, double distance)
        {
            distance = Math.Round(distance, 4);
            double from = Math.Round(Settings.Default.SpecialDistanceFrom, 4);
            double to = Math.Round(Settings.Default.SpecialDistanceTo, 4);
            bool specialDistanceHit = (to == 0)
                ? distance == from && from > 0
                : from <= distance && distance <= to;

            if (Settings.Default.SpecialDistanceLabel && specialDistanceHit)
            {
                LabelStorage.WriteLabel(LabelType.SpecialDistance, player.FullDisplayName, LabelPath);
            }

            if (Settings.Default.SpecialDistanceAction && !string.IsNullOrEmpty(Settings.Default.SpecialDistanceActionID) && Settings.Default.SpecialDistanceActionName != "[none]" && specialDistanceHit)
            {
                StreamerbotClient.ExecuteAction(Settings.Default.SpecialDistanceActionID, Settings.Default.SpecialDistanceActionName);
            }
            if (Settings.Default.ObsSpecialDistanceExecute && !string.IsNullOrEmpty(Settings.Default.ObsSpecialDistanceSource) && !string.IsNullOrEmpty(Settings.Default.ObsSpecialDistanceAction) && specialDistanceHit)
            {
                obsClient.ModifySource(Settings.Default.ObsSpecialDistanceScene, Settings.Default.ObsSpecialDistanceSource, Settings.Default.ObsSpecialDistanceAction);
            }
        }

    }
}
