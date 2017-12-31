using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Microsoft.Phone.Shell;
using System.Linq;

namespace SharedCode
{
    public class ChangeTileInfo
    {
        public void ChangeBackOfTile()
        {
            List<string> quotes = new List<string>();
            quotes.Add("Come write a macro");
            quotes.Add("where are you?");
            quotes.Add("cant play wow?");
            quotes.Add("why no macros?");
            // Get the text lines.
            //string[] lines = quotes;
            // Make a Random object.
            Random rand = new Random();
            // Get a random index between
            // 0 inclusive and lines.Length exclusive.
            int index = rand.Next(0, quotes.Count - 1);
            // Display the result.
            string quote = quotes[index];
            ShellTile TileToFind = ShellTile.ActiveTiles.First();
            if (TileToFind != null)
            {
                StandardTileData NewTileData = new StandardTileData();
                //NewTileData.Title = "My Cool Tile";
                //NewTileData.BackgroundImage = new Uri("CoolTileFromPush.jpg", UriKind.Relative);
                NewTileData.Count = rand.Next(10);
                NewTileData.BackContent = quote;
                //NewTileData.BackBackgroundImage = new Uri("CoolTileBack.jpg", UriKind.Relative);
                TileToFind.Update(NewTileData);
            }
        }




    }
}
