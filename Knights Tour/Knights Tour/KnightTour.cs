using System;
using System.Collections.Generic;

namespace Knights_Tour
{
    public class KnightTour
    {
        private int N = 8;
        private readonly int[,] _board;
        private int _nextX, _nextY;
        private readonly int _curX, _curY;
        private readonly List<Moves> _moves;
        private readonly int[,] _possibleMoves = {
            { 2, 1 },
            { 2, -1 },
            { 1, 2 },
            { 1, -2 },
            { -2, 1 },
            { -2, -1 },
            { -1, 2 },
            { -1, -2 }
        };
        //Satranç matrix ini oluşturmak
        public KnightTour(int x, int y)
        {
            _board = new int[N, N]; // 8x8 lik matrix oluşturdu
            _moves = new List<Moves>(); // Sonraki hareket kaydetmek için listede oluşturdu
            _board.Initialize(); // bu kod matrixdeki her yeri 0(default) yapar
            do
            {
                _curX = x;
                _curY = y;
            } while (!FindClosedTour());
        }
        //X,Y koordinatları satranç tahtasınının sınırları içerisinde kontrolü
        private bool IsValid(int x, int y) // Geçerli mi?
        {
            return ((x >= 0 && y >= 0) && (x < N && y < N));
        }
        //X,Y koordinatları boş oludugunu kontrolü
        private bool IsEmpty(int[,] board, int x, int y) // Boş Mu ?
        {
            return (IsValid(x, y)) && (board[x, y] == 0); // x y noktaları santraç içerisinde mi ve bu nokta boş mu ?
        }
        //X,Y Kordinatlarından muhtemel gidiş yol sayısını bulma (Derece)
        private int GetDegree(int[,] board, int x, int y) // Kaç muhtemlen giidş yolun var ve bu yolların kaçı boş.
        {
            int count = 0;
            for (int i = 0; i < N; ++i)
            {
                if (IsEmpty(board, (x + _possibleMoves[i, 0]), (y + _possibleMoves[i, 1])))
                {
                    count++;
                }
            }
            return count;
        }
        //Bir sonra ki adımı bulma
        /*
         Listeden rastgele bir yol sececez ve bu yol en az derece sahip olması gerekir
             Önce şuanki adımdanda sonraki seçilen adım dolumu boşmu ona bak sonra derece en aza git
             */
        private bool NextMove(int[,] board, int x, int y) // x ve y şuan ki nokta ,bu fonk. sonraki en uygun notayı bulmayı çalışır
        {
            int minDegIdx = -1, c, minDeg = (N + 1), nx, ny;
            int start = new Random().Next(N); // Gidiş yönlerinden rastgele birini seçme
            for (int count = 0; count < N; ++count) // kac derecesi var
            {
                int i = (start + count) % N; // randomsal seçim
                nx = x + _possibleMoves[i, 0]; // şuanki nokta + şeçilen nokta
                ny = y + _possibleMoves[i, 1]; // şuanki nokta + şeçilen nokta
                if ((IsEmpty(board, nx, ny)) && (c = GetDegree(board, nx, ny)) < minDeg) //En az muhtemel yolu olan koordinatı seçme
                {
                    minDegIdx = i;
                    minDeg = c;
                }
            }
            if (minDegIdx == -1)
            {
                return false;
            }
            //Gerekli adımı işaretleme
            nx = x + _possibleMoves[minDegIdx, 0];
            ny = y + _possibleMoves[minDegIdx, 1];
            board[nx, ny] = board[x, y] + 1;
            _moves.Add(new Moves { X = nx, Y = ny, Order = board[nx, ny] });
            _nextX = nx;
            _nextY = ny;
            return true;
        }
        //Verilen iki koordinat birbirlerine komşuluğunu kontrol etme
        private bool Neighbour(int x, int y, int xx, int yy)
        {
            for (int i = 0; i < N; ++i)
                if (((x + _possibleMoves[i, 0]) == xx) && ((y + _possibleMoves[i, 1]) == yy))
                    return true;

            return false;
        }
        //Gidiş yolunu bulma
        // ilk çalışan fonksiyon
        private bool FindClosedTour()
        {
            _nextX = _curX;
            _nextY = _curY;
            _board[_curX, _curY] = 1; // Şuanki adımı 1 yaptık
            _moves.Add(new Moves { X = _curX, Y = _curY, Order = 1 }); // Bu adımı kaydedik
            //64 adımı tamamlıyana kadar çöz
            for (int i = 0; i < N * N - 1; ++i)
            {
                if (!NextMove(_board, _nextX, _nextY))
                {
                    return false;
                }
            }
            //Kontrol et
            if (Neighbour(_nextX, _nextY, _curX, _curY))
            {
                return false;
            }
            return true;
        }
        public List<Moves> GetMoves()
        {
            return _moves;
        }
        public int[,] GetBoard()
        {
            return _board;
        }
    }
}
