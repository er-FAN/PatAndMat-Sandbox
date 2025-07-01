using Simulation.Engine.Components.Render;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Simulation.App.Models
{
    public class WpfAnimatable : IAnimatable
    {
        public Dictionary<string, List<string>> Sprites { get; set; } = [];
        public List<ILogic> Logics { get; set; } = [];
        public int CurrentSpriteIndex { get; set; } = 0;
        public string CurrentSpriteAddres { get; set; } = string.Empty;
        public BitmapImage CurrentSprite { get; set; } = new BitmapImage();


    }

    
}
