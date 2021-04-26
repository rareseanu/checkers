using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2.Models
{
    public class Piece : INotifyPropertyChanged
    {
        private bool isKing = false;
        public bool IsKing
        {
            get { return isKing; }
            set
            {
                isKing = value;
                if (isKing)
                {
                    if (color == ColorType.WHITE)
                    {
                        image = "/Tema2;component/Resources/whiteKing.png";
                    }
                    else
                    {
                        image = "/Tema2;component/Resources/redKing.png";
                    }
                }
                NotifyPropertyChanged("IsKing");
                NotifyPropertyChanged("Image");
            }
        }
        public enum ColorType
        {
            RED,
            WHITE
        }
        public Piece(ColorType color)
        {
            this.color = color;
            if(color == ColorType.RED)
            {
                image = "/Tema2;component/Resources/red.png";
            } else
            {
                image = "/Tema2;component/Resources/white.png";
            }
        }

        private ColorType color;
        public ColorType Color
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                NotifyPropertyChanged("Image");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
