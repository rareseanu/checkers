using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Tema2.Models;
using Tema2.ViewModels;

namespace Tema2.Services
{
    public class CheckersGameLogic : INotifyPropertyChanged
    {
        private Tuple<int, int> statistics;
        public Tuple<int, int> Statistics
        {
            get { return statistics; }
            set
            {
                statistics = value;
                NotifyPropertyChanged("Statistics");
            }
        }

        private string redName = "Red";
        public string RedName
        {
            get { return redName; }
            set
            {
                redName = value;
                NotifyPropertyChanged("RedName");
            } 
        }
        private string whiteName = "White";
        public string WhiteName
        {
            get { return whiteName; }
            set
            {
                whiteName = value;
                NotifyPropertyChanged("WhiteName");
            }
        }

        public Cell CurrentCell { get; set; }
        public Player red { get; set; }
        public Player white { get; set; }
        public Player currentTurn { get; set; }
        private bool repeatTurn { get; set; }
        private bool multipleJumps = true;
        public bool MultipleJumps
        {
            get { return multipleJumps; }
            set
            {
                multipleJumps = value;
                NotifyPropertyChanged("MultipleJumps");
            }
        }
        private string gameStatus;
        public string GameStatus
        {
            get { return gameStatus; }
            set
            {
                gameStatus = value;
                NotifyPropertyChanged("GameStatus");
            }
        }

        ObservableCollection<CellVM> board;
        List<CellVM> hints;
        public CheckersGameLogic(ObservableCollection<CellVM> board, List<CellVM> hints)
        {
            this.board = board;
            this.hints = hints;
            red = new Player(RedName, Piece.ColorType.RED);
            white = new Player(WhiteName, Piece.ColorType.WHITE);
            currentTurn = red;
            GameStatus = $"{currentTurn.Name}'s turn";
        }

        public void ResetGameLogic()
        {
            red.PiecesTaken = 0;
            white.PiecesTaken = 0;
            currentTurn = red;
            GameStatus = $"{currentTurn.Name}'s turn";
        }

        CellVM GetCellAtCoords(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return null;
            }
            return board[x * 8 + y];
        }

        void AddValidHint(CellVM cell)
        {
            if (cell != null && cell.SimpleCell.Piece == null)
            {
                hints.Add(cell);
                cell.SimpleCell.Color = Colors.LawnGreen;
            }
        }

        void ClearHints()
        {
            for (int i = 0; i < hints.Count; ++i)
            {
                CellVM currentCell = hints[i];
                if ((currentCell.SimpleCell.X % 2) == (currentCell.SimpleCell.Y % 2))
                {
                    currentCell.SimpleCell.Color = (Color)ColorConverter.ConvertFromString("#eeeed4");
                }
                else
                {
                    currentCell.SimpleCell.Color = (Color)ColorConverter.ConvertFromString("#7d945c");
                }
            }
            hints.Clear();
        }

        public void Select(Cell currentCell)
        {
            if (currentCell.Piece != null && !repeatTurn) // Alegere piesa de mutat
            {
                if (currentTurn.Color == currentCell.Piece.Color)
                {
                    if (CurrentCell != null)
                    {
                        ClearHints();
                    }
                    CurrentCell = currentCell;
                    int difference = 1, differencev2 = 2;
                    if (currentTurn == red)
                    {
                        difference = -1;
                        differencev2 = -2;
                    }
                    else
                    {
                        difference = 1;
                        differencev2 = 2;
                    }
                    CellVM colt1 = GetCellAtCoords(CurrentCell.X + difference, CurrentCell.Y + 1); //colt1
                    AddValidHint(colt1);
                    CellVM colt2 = GetCellAtCoords(CurrentCell.X + difference, CurrentCell.Y - 1); //colt2
                    AddValidHint(colt2);
                    CellVM colt1Saritura = GetCellAtCoords(CurrentCell.X + differencev2, CurrentCell.Y + 2);
                    if (colt1Saritura != null && colt1 != null)
                    {
                        if (colt1.SimpleCell.Piece != null && colt1.SimpleCell.Piece.Color != currentTurn.Color)
                        {
                            AddValidHint(colt1Saritura);
                        }
                    }
                    CellVM colt2Saritura = GetCellAtCoords(CurrentCell.X + differencev2, CurrentCell.Y - 2);
                    if (colt2Saritura != null && colt2 != null)
                    {
                        if (colt2.SimpleCell.Piece != null && colt2.SimpleCell.Piece.Color != currentTurn.Color)
                        {
                            AddValidHint(colt2Saritura);
                        }
                    }
                    if (CurrentCell.Piece.IsKing)
                    {
                        CellVM kcolt1 = GetCellAtCoords(CurrentCell.X - difference, CurrentCell.Y + 1); //colt1
                        AddValidHint(kcolt1);
                        CellVM kcolt2 = GetCellAtCoords(CurrentCell.X - difference, CurrentCell.Y - 1); //colt2
                        AddValidHint(kcolt2);
                        CellVM kcolt1Saritura = GetCellAtCoords(CurrentCell.X - differencev2, CurrentCell.Y + 2);
                        if (kcolt1Saritura != null && kcolt1 != null)
                        {
                            if (kcolt1.SimpleCell.Piece != null && kcolt1.SimpleCell.Piece.Color != currentTurn.Color)
                            {
                                AddValidHint(kcolt1Saritura);
                            }
                        }
                        CellVM kcolt2Saritura = GetCellAtCoords(CurrentCell.X - differencev2, CurrentCell.Y - 2);
                        if (kcolt2Saritura != null && kcolt2 != null)
                        {
                            if (kcolt2.SimpleCell.Piece != null && kcolt2.SimpleCell.Piece.Color != currentTurn.Color)
                            {
                                AddValidHint(kcolt2Saritura);
                            }
                        }
                    }
                }
                else
                {
                    CurrentCell = null; // Jucatorul curent a ales piesa celuilalt jucator.
                    ClearHints();

                }
            }
            else // Mutare piesa aleasa
            {
                if (CurrentCell != null && hints.Find(x => x.SimpleCell.X == currentCell.X && x.SimpleCell.Y == currentCell.Y) != null)
                {
                    currentCell.Piece = CurrentCell.Piece;

                    if (currentCell.X == 0 && !currentCell.Piece.IsKing && currentTurn.Color == Piece.ColorType.RED)
                    {
                        currentCell.Piece.IsKing = true;
                    }
                    else if (currentCell.X == 7 && !currentCell.Piece.IsKing && currentTurn.Color == Piece.ColorType.WHITE)
                    {
                        currentCell.Piece.IsKing = true;
                    }
                    ClearHints();
                    if (Math.Abs(CurrentCell.X - currentCell.X) == 2)
                    {
                        GetCellAtCoords((CurrentCell.X + currentCell.X) / 2, (CurrentCell.Y + currentCell.Y) / 2).SimpleCell.Piece = null;
                        currentTurn.PiecesTaken++;
                        if (currentTurn.PiecesTaken == 12)
                        {
                            GameStatus = currentTurn.Name + " won.";
                            if(currentTurn.Color == Piece.ColorType.RED)
                            {
                                Statistics = Tuple.Create(statistics.Item1 + 1, statistics.Item2);
                            } else
                            {
                                Statistics = Tuple.Create(statistics.Item1, statistics.Item2 + 1);
                            }
                            SaveStatistics();
                            CurrentCell.Piece = null;
                            CurrentCell = currentCell;
                            return;
                        }
                        if (multipleJumps)
                        {
                            int difference = 1, differencev2 = 2;
                            if (currentTurn == red)
                            {
                                difference = -1;
                                differencev2 = -2;
                            }
                            else
                            {
                                difference = 1;
                                differencev2 = 2;
                            }
                            CellVM colt1 = GetCellAtCoords(currentCell.X + difference, currentCell.Y + 1); //colt1
                            CellVM colt2 = GetCellAtCoords(currentCell.X + difference, currentCell.Y - 1); //colt2
                            CellVM colt1Saritura = GetCellAtCoords(currentCell.X + differencev2, currentCell.Y + 2);
                            if (colt1Saritura != null && colt1 != null)
                            {
                                if (colt1.SimpleCell.Piece != null && colt1.SimpleCell.Piece.Color != currentTurn.Color)
                                {
                                    AddValidHint(colt1Saritura);
                                }
                            }
                            CellVM colt2Saritura = GetCellAtCoords(currentCell.X + differencev2, currentCell.Y - 2);
                            if (colt2Saritura != null && colt2 != null)
                            {
                                if (colt2.SimpleCell.Piece != null && colt2.SimpleCell.Piece.Color != currentTurn.Color)
                                {
                                    AddValidHint(colt2Saritura);
                                }
                            }
                            if (CurrentCell.Piece.IsKing)
                            {
                                CellVM kcolt1 = GetCellAtCoords(currentCell.X - difference, currentCell.Y + 1); //colt1
                                CellVM kcolt2 = GetCellAtCoords(currentCell.X - difference, currentCell.Y - 1); //colt2
                                CellVM kcolt1Saritura = GetCellAtCoords(currentCell.X - differencev2, currentCell.Y + 2);
                                if (kcolt1Saritura != null && kcolt1 != null)
                                {
                                    if (kcolt1.SimpleCell.Piece != null && kcolt1.SimpleCell.Piece.Color != currentTurn.Color)
                                    {
                                        AddValidHint(kcolt1Saritura);
                                    }
                                }
                                CellVM kcolt2Saritura = GetCellAtCoords(currentCell.X - differencev2, currentCell.Y - 2);
                                if (kcolt2Saritura != null && kcolt2 != null)
                                {
                                    if (kcolt2.SimpleCell.Piece != null && kcolt2.SimpleCell.Piece.Color != currentTurn.Color)
                                    {
                                        AddValidHint(kcolt2Saritura);
                                    }
                                }
                            }
                        }

                    }
                    if (hints.Count != 0)
                    {
                        CurrentCell.Piece = null;
                        CurrentCell = currentCell;
                        repeatTurn = true;
                        return;
                    }
                    repeatTurn = false;
                    CurrentCell.Piece = null;
                    if (currentTurn == red)
                    {
                        currentTurn = white;
                    }
                    else
                    {
                        currentTurn = red;
                    }
                    GameStatus = currentTurn.Name + "'s turn";
                }
            }
        }

        void SaveStatistics()
        {
            var xdoc = new XDocument(
             new XElement("stats",
                 new XElement("redWins", statistics.Item1.ToString()),
                 new XElement("whiteWins", statistics.Item2.ToString())));
            xdoc.Save("statistics.xml");
        }

        public void ClickAction(Cell obj)
        {
            Select(obj);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}