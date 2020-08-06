using System.IO;
using System.Windows.Media.Imaging;
using Client.Models;

namespace Client.Services.HttpService
{
    // need to serialize and send as json 
    public class NetworkCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }

        public NetworkCard()
        {

        }
        public NetworkCard(Card card)
        {
            Id = card.Id; 
            Title = card.Title;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(card.Image));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                Image= ms.ToArray();
            }
        }

        public Card ToCard()
        {
            var result = new Card()
            {
                Id = Id,
                Title = Title,
            };
            // convert from bytes
            using (var ms = new MemoryStream(Image))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; 
                image.StreamSource = ms;
                image.EndInit();
                result.Image = image;
            }
            
            return result;
        }
    }
}