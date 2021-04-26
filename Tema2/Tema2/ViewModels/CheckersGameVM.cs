using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Tema2.Commands;
using Tema2.Models;
using Tema2.Services;

namespace Tema2.ViewModels
{
    public class CheckersGameVM
    {
        public int columns = 8;
        public int rows = 8;

        public CheckersGameVM()
        {
            GameBoard = new ObservableCollection<CellVM>();
            Hints = new List<CellVM>();
            logic = new CheckersGameLogic(GameBoard, Hints);
            for (int i = 0; i < rows; ++i)
            {
                for(int j = 0; j < columns; ++j)
                {
                    Color backgroundColor;
                    Piece newPiece = null;
                    if((i % 2) == (j % 2))
                    {
                        backgroundColor = (Color)ColorConverter.ConvertFromString("#eeeed4");
                    } else
                    {
                        backgroundColor = (Color)ColorConverter.ConvertFromString("#7d945c");
                        if (i > 4)
                        {
                            newPiece = new Piece(Piece.ColorType.RED);
                        }
                        if (i < 3)
                        {
                            newPiece = new Piece(Piece.ColorType.WHITE);
                        }
                    }
                    CellVM newCell = new CellVM(i, j, backgroundColor, logic);
                    if(newPiece != null)
                    {
                        newCell.SimpleCell.Piece = newPiece;
                    }
                    GameBoard.Add(newCell);
                }
            }
        }

        public CheckersGameLogic logic { get; set; }
        public ObservableCollection<CellVM> GameBoard { get; set; }
        public List<CellVM> Hints { get; set; }

    }
}
