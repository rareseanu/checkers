using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Tema2.Commands;
using Tema2.Models;
using Tema2.Services;

namespace Tema2.ViewModels
{
    public class CellVM : INotifyPropertyChanged
    {
        private CheckersGameLogic logic;
        public CellVM(int x, int y, Color color, CheckersGameLogic logic)
        {
            SimpleCell = new Cell(x, y, color);
            this.logic = logic;
        }
        private Cell simpleCell;
        public Cell SimpleCell
        {
            get { return simpleCell; }
            set
            {
                simpleCell = value;
                NotifyPropertyChanged("SimpleCell");
            }
        }

        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Cell>(logic.ClickAction);
                }
                return clickCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
