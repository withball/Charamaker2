using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charamaker2
{

    //ソートするためのノード
    class sortnode<T>
        where T:class
    {
        /// <summary>
        /// ソート対象のオブジェクト
        /// </summary>
        public T o;
        /// <summary>
        /// ソートするための値
        /// </summary>
        public float v;
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="obj">ソートするオブジェクト</param>
        /// <param name="value">その値</param>
        public sortnode(T obj, float value)
        {
            o = obj;
            v = value;
        }

    };

    /// <summary>
    /// 任意の物体をソートするためのクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class supersort<T>
        where T:class
    {
        List<sortnode<T>> list=new List<sortnode<T>>();
        /// <summary>
        /// ソートするものを追加する
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="value">ソートするための値</param>
        public void add(T obj, float value)
        {
            list.Add(new sortnode<T>(obj, value));
        }
        /// <summary>
        /// ソートを行う
        /// </summary>
        /// <param name="ookiijyunn">大きい順に並べるか</param>
        public void sort(bool ookiijyunn)
        {
            if (list.Count <= 0) return;
            List<List<sortnode<T>>> sorts = new List<List<sortnode<T>>>();
            int si = list.Count();
            int i, p, q;
            sortnode<T> temp;
            int tsi = 0;
            while (true)
            {
                sorts.Add(new List<sortnode<T>>());
                for (i = 0; i < 64; i++)
                {
                    if (tsi * 64 + i >= si)
                    {
                        break;
                    }
                    sorts[tsi].Add(list[tsi * 64 + i]);

                }

                if (tsi * 64 + i >= si)
                {

                    break;
                }
                tsi++;


            }

            for (i = 0; i <= tsi; i++)
            {

                for (p = 1; p < sorts[i].Count; p++)
                {

                    temp = sorts[i][p];
                    if (sorts[i][p - 1].v < temp.v^!ookiijyunn)
                    {
                        q = p;
                        do
                        {
                            sorts[i][q] = sorts[i][q - 1];
                            q--;
                        } while (q > 0 && (sorts[i][q - 1].v < temp.v ^ !ookiijyunn));
                        sorts[i][q] = temp;
                    }
                }

            }

            while (sorts.Count > 1)
            {
                p = 1;
                while (p < sorts.Count)
                {
                    i = 0; q = 0;
                    //マージ


                    while (i < sorts[p - 1].Count)
                    {
                        if (sorts[p - 1][i].v < sorts[p][q].v ^ !ookiijyunn)
                        {
                            sorts[p - 1].Insert(i, sorts[p][q]);
                            i++;
                            q++;
                        }
                        else
                        {
                            i++;
                        }
                        if (q >= sorts[p].Count) break;
                    }
                    while (q < sorts[p].Count)
                    {
                        sorts[p - 1].Add(sorts[p][q]);
                        q++;
                    }
                    //戦後処理
                    sorts.RemoveAt(p);
                    if (sorts.Count() - 1 == p)
                    {
                    }
                    else p++;
                }


            }
            list = sorts[0];
        }
        /// <summary>
        /// 物体を交換する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void swap(int x, int y)
        {
            var temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }
        /// <summary>
        /// 物体を指定位置に挿入するように移動させる
        /// </summary>
        /// <param name="koitu">物体のインデックス</param>
        /// <param name="kokoni">挿入する位置</param>
        void conveyor(int koitu, int kokoni)
        {
            //	TRACE(_T("%d %d conv\n "), koitu,kokoni);
            var temp = list[koitu];
            if (koitu < kokoni)
            {
                for (int i = koitu; i < kokoni; i++)
                {
                    swap(i, i + 1);
                }
            }
            else
            {
                for (int i = koitu; i > kokoni; i--)
                {
                    swap(i, i - 1);
                }
            }
            list[kokoni] = temp;
        }
        /// <summary>
        /// ソートされたベクトルを受け取る
        /// </summary>
        /// <param name="back">逆にして受け取るか</param>
        /// <returns></returns>
       public List<T> getresult(bool back = false)
        {
            List<T> res=new List<T>();
            if (!back)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    res.Add(list[i].o);
                    //	TRACE(_T("%f "),sorts[i].v);
                }
            }
            else
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    res.Add(list[i].o);
                }
            }
            //TRACE(_T("DA\n"));
            return res;
        }
    };

}
