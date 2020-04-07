using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using WvW_Status.Utilities;

namespace WvW_Status
{
    public partial class MainWindow : Window
    {
        private static readonly List<Team> Teams = new List<Team>();
        private static readonly Dictionary<int, World> Worlds = new Dictionary<int, World>();

        public MainWindow()
        {
            InitializeComponent();
            
            var client = new WebClient();
            var worldsData = client.DownloadString("https://api.guildwars2.com/v2/worlds?ids=all");
            var worldResult = JsonConvert.DeserializeObject<List<World>>(worldsData);

            var matchesData = client.DownloadString("https://api.guildwars2.com/v2/wvw/matches?ids=all");
            var matchesResult = JsonConvert.DeserializeObject<List<Match>>(matchesData);
            client.Dispose();

            foreach (var world in worldResult)
            {
                Worlds.Add(world.Id, new World() { Id = world.Id, Name = world.Name, Population = world.Population });
            }

            foreach (var match in matchesResult)
            {
                CreateTeamInfo(match, "Green", Color.FromArgb(255, 0, 100, 0));
                CreateTeamInfo(match, "Blue", Color.FromArgb(255, 0, 0,139));
                CreateTeamInfo(match, "Red", Color.FromArgb(255,139, 0, 0));
            }
            
        }
        private void CreateTeamInfo(Match match, string teamColor, Color displayColor)
        {
            string GenerateMatchInfoTip(IEnumerable<int> list)
            {
                return list.Reverse().Aggregate(
                    "",
                    (current, id) => current + (Worlds[id].Name.PadRight(25) + "\t" + Worlds[id].Population + "\r\n")
                );
            }

            var vp = Util.GetPropertyValue<int>(match.Victory_Points, teamColor);
            var hvp = vp + ((85 - match.Skirmishes.Count) * 5);
            var lvp = vp + ((85 - match.Skirmishes.Count) * 3);

            Teams.Add(new Team()
            {
                Region = match.Id.Substring(0, 1) == "1" ? "NA" : "EU",
                Tier = int.Parse(match.Id.Substring(match.Id.Length - 1, 1)),
                Name = Worlds[Util.GetPropertyValue<int>(match.Worlds, teamColor)].Name,
                Color = displayColor,
                Image = null,
                VP = vp,
                HVP = hvp,
                LVP = lvp,
                Score = Util.GetPropertyValue<int>(match.Skirmishes.LastOrDefault().Scores, teamColor),
                VP_Tip = "Highest\t " + hvp.ToString() + "\r\n" + "Lowest\t " + lvp.ToString(),
                Link_Tip = GenerateMatchInfoTip(Util.GetPropertyValue<IEnumerable<int>>(match.All_Worlds, teamColor))
            });
        }
    }
}