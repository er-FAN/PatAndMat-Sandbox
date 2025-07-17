using Simulation.Engine.models;
using System.Windows.Media.Imaging;

namespace Simulation.App.Models
{
    public class GetNextSpriteLogic(string state) : ILogic
    {
        public string State = state;
        WpfAnimatable Animatable = new();
        public void Apply(ISimulableObject simulableObject, IContext context)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            Animatable = simulableObject.GetComponent<WpfAnimatable>();
#pragma warning restore CS8601 // Possible null reference assignment.
            if (Animatable != null)
            {
                Animatable.CurrentSpriteAddres = Animatable.Sprites[State].ToList()[Animatable.CurrentSpriteIndex];
                Animatable.CurrentSprite = LoadImage(Animatable.CurrentSpriteAddres);
                Animatable.CurrentSpriteIndex++;

                if (Animatable.CurrentSpriteIndex >= Animatable.Sprites[State].Count)
                {
                    Animatable.CurrentSpriteIndex = 0;
                }
            }
        }

        BitmapImage LoadImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }
    }

}