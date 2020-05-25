using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiZhuaChart.NET.Class_API
{
    public class Analyzer
    {
        public static Dictionary<string, int> AnalyzeParties(List<Party> parties)
        {
            Dictionary<string, int> dt = new Dictionary<string, int>();
            var partyGroups = from party in parties group party by party.name;
            foreach(var partyGroup in partyGroups)
            {
                dt.Add(partyGroup.Key, partyGroup.Count());
            }
            return dt;
        }

        public static Dictionary<string, int> AnalyzeNickname(List<Party> parties)
        {
            Dictionary<string, int> dt = new Dictionary<string, int>();
            var partyGroups = from party in parties group party by party.user_nickname;
            partyGroups = from party in partyGroups orderby party.Count() descending select party;
            var partyList = partyGroups.ToList();
            for(int i = 0; i < 20; i++)
            {
                dt.Add(partyList[i].Key, partyList[i].Count());
            }
            return dt;
        }

        public static Dictionary<string, int> AnalyzeFriends(List<Friend> friends)
        {
            Dictionary<string, int> dt = new Dictionary<string, int>();
            var friendGroups = from friend in friends group friend by friend.close_grade;
            foreach(var friendGroup in friendGroups)
            {
                dt.Add(friendGroup.Key.ToString(), friendGroup.Count());
            }
            return dt;
        }


    }
}