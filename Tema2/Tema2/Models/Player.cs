using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tema2.Models.Piece;

namespace Tema2.Models
{
    public class Player : INotifyPropertyChanged
    {
        public Player(string name, ColorType color)
        {
            this.name = name;
            this.color = color;
            this.piecesTaken = 0;
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        private int piecesTaken;
        public int PiecesTaken
        {
            get { return piecesTaken; }
            set
            {
                piecesTaken = value;
                NotifyPropertyChanged("PiecesTaken");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
