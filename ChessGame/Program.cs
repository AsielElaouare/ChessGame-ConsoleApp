using System.Diagnostics;
using System.Linq.Expressions;

namespace ChessGameConsoleApp { 

    internal class Program
	{
		const int ROWS = 8;
		const int COLUMNS = 8;
		static void Main(string[] args)
		{
			Program program = new Program();    
			program.Start();
		}

		void Start()
		{
			ChessPiece[,] chessboard = new ChessPiece[ROWS, COLUMNS];
			InitChessboard(chessboard);
			DisplayChessboard(chessboard);
			PlayChess(chessboard);


		}


		

		void InitChessboard(ChessPiece[,] chessboard)
		{
			for (int row = 0; row < chessboard.GetLength(0); row++)
			{
				for (int col = 0; col < chessboard.GetLength(1); col++)
				{
					chessboard[row, col] = null;
				}
			}

			PutChessPieces(chessboard);
		}

		void CheckMove(ChessPiece[,] chessboard, Position from, Position to)
		{
            if (chessboard[from.Row, from.Column] != null)
            {
                int hor = Math.Abs(to.Column - from.Column);
                int ver = Math.Abs(to.Row - from.Row);

                if (hor == 0 && ver == 0)
                {
					DisplayError("No move");
                }
				else if (chessboard[to.Row, to.Column] != null && chessboard[from.Row, from.Column].color == chessboard[to.Row, to.Column].color)
				{
					DisplayError("Can not take chess piece of same color ");

				}
				else if (chessboard[to.Row, to.Column] == null || chessboard[to.Row, to.Column] != null && chessboard[from.Row, from.Column].color != chessboard[to.Row, to.Column].color)
				{
					try
					{


						switch (chessboard[from.Row, from.Column].type)
						{
							case ChessPieceType.Rook:
								if (hor * ver != 0)
									throw new Exception($"Invalid move for chess piece {ChessPieceType.Rook}");
								break;

							case ChessPieceType.Knight:
								if (hor * ver != 2)
									throw new Exception($"Invalid move for chess piece {ChessPieceType.Knight}");
								break;

							case ChessPieceType.Bishop:
								if (hor != ver)
									throw new Exception($"Invalid move for chess piece {ChessPieceType.Bishop}");
								break;

							case ChessPieceType.King:
								if ((hor == 1 && ver == 0) || (hor == 0 && ver == 1) || (hor == 1 && ver == 1))
									break;
								throw new Exception($"Invalid move for chess piece {ChessPieceType.King}");

							case ChessPieceType.Queen:
								if (hor * ver != 0 && hor != ver)
									throw new Exception($"Invalid move for chess piece {ChessPieceType.Queen}");
								break;

							case ChessPieceType.Pawn:
								if (hor == 0 && ver == 1)
									break;
								throw new Exception($"Invalid move for chess piece {ChessPieceType.Pawn}");

							default:
								throw new Exception("Unknown chess piece type");
						}

                        chessboard[to.Row, to.Column] = chessboard[from.Row, from.Column];
                        chessboard[from.Row, from.Column] = null;
                    }
					catch (Exception ex)
					{
						DisplayError($"{ex.Message}");
					}
                    
                }
            }
            else
            {
                DisplayError("No chess piece at from-position");
            }

        }


		void DoMove(ChessPiece[,] chessboard, Position from, Position to)
		{
			CheckMove(chessboard, from, to);
			
		}

		void PlayChess(ChessPiece[,] chessboard)
		{
			string inputMove;
			
			do
			{
				inputMove = ReadPosition();

				if (inputMove != "stop")
				{
					string[] moves = inputMove.Split(' ');
					Position position0 = String2Position(moves[0]);
					Position position1;
					if (position0 != null)
					{
						position1 = String2Position(moves[1]);

                        Console.WriteLine($"move from {moves[0]} to {moves[1]}");
                        DoMove(chessboard, position0, position1);
                        DisplayChessboard(chessboard);
                    }

					
					
						
					
				}
				Console.WriteLine();
			} while (inputMove != "stop");

		}

		Position String2Position(string pos)
		{
			

			Position position = new Position();

			int column = pos[0] - 'a';
			int row = 8 - int.Parse(pos[1].ToString());

		
			if (column >= 0 && column < 8)
			{
				position.Column = column;
			}
			else 
			{
				DisplayError($"Invalid postion: {pos}");
				return null;
			}

			if (row >= 0 && row < 8)
			{
				position.Row = row;
			}
			else
			{
				DisplayError($"Invalid postion: {pos}");
				return null;

			}

			return position;
		}


		void DisplayChessPiece(ChessPiece chessPiece, int row)
		{
			if (chessPiece != null && chessPiece.color == ChessPieceColor.black)
			{
				Console.ForegroundColor = ConsoleColor.Black;
				
				Console.Write($" {chessPiece.type.ToString().ToLower()[0]} ");
				Console.ResetColor();

			}
			else if (chessPiece != null && chessPiece.color == ChessPieceColor.white)
			{
				Console.ForegroundColor = ConsoleColor.White;
				
				Console.Write($" {chessPiece.type.ToString().ToLower()[0]} ");
				Console.ResetColor();

			}
			else if (chessPiece == null)
			{
				Console.Write("   ");
			}

			

		}

		void DisplayChessboard(ChessPiece[,] chessboard)
		{
			int rowNumber = ROWS + 1;
			for (int row = 0; row < chessboard.GetLength(0); row++)
			{
				rowNumber--;
				Console.Write(rowNumber + " ");

				for (int col = 0; col < chessboard.GetLength(1); col++)
				{
					if ((row + col) % 2 == 0)
					{
						Console.BackgroundColor = ConsoleColor.DarkGray;
						DisplayChessPiece(chessboard[row, col], row);
						Console.ResetColor();

					}
					else
					{
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						DisplayChessPiece(chessboard[row, col], row);
						Console.ResetColor();
					}
				}
				Console.WriteLine();
			}
			Console.ResetColor();
			// write about this soclution on blog
			Console.WriteLine("   a  b  c  d  e  f  g  h");

		}


		void PutChessPieces(ChessPiece[,] chessboard)
		{
			ChessPieceType[] order = { ChessPieceType.Rook, ChessPieceType.Knight, ChessPieceType.Bishop, ChessPieceType.Queen,
									   ChessPieceType.King, ChessPieceType.Bishop, ChessPieceType.Knight, ChessPieceType.Rook };

			for (int row = 0; row < chessboard.GetLength(0); row++)
			{
				for (int col = 0; col < chessboard.GetLength(1); col++)
				{
					/// <summary>
					/// explain the solutoion of line number 83
				 /// </summary>


					chessboard[row, col] = new ChessPiece();

					if (row == 0)
					{
						chessboard[row, col].type = order[col];
						chessboard[row, col].color = ChessPieceColor.black;



					}
					else if (row == 1)
					{
						chessboard[row, col].type = ChessPieceType.Pawn;
						chessboard[row, col].color = ChessPieceColor.black;
					}
					else if (row == 7)
					{
						chessboard[row, col].type = order[col];
						chessboard[row, col].color = ChessPieceColor.white;
					}
					else if (row == 6)
					{
						chessboard[row, col].type = ChessPieceType.Pawn;
						chessboard[row, col].color = ChessPieceColor.white;
					}
					else
					{
						chessboard[row, col] = null;
					}
				}
			}
		}

		static string ReadPosition()
		{
			Console.WriteLine("Enter a move (e.g. a2 a3): ");
			return Console.ReadLine();
		}



		void DisplayError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}