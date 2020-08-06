using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Client.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public BitmapImage Image { get; set; }
    }
}