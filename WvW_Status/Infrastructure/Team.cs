using System.Windows.Controls;
using System.Windows.Media;

namespace WvW_Status
{
    public class Team
    {
        public string Region { get; set; }      // NA or EU
        public int Tier { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }        // Team Color
        public Image Image { get; set; }        // For Lock
        public int VP { get; set; }             // Victory Points   
        public int HVP { get; set; }            // Highest Possible Victory Points
        public int LVP { get; set; }            // Lowest Possible Victory Points
        public int Score { get; set; }          // Current Skirmish Warscore
        public string VP_Tip { get; set; }      // Victory Point Tooltip
        public string Link_Tip { get; set; }    // Linked Server Tooltip
    }
}