using Charamaker2;
using Charamaker2.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameSet1
{
    /// <summary>
    /// エンテティマネージャーのためのデータベース。Entityを継承した場合はEntityManagerからタイプを変えてくれや
    /// </summary>
    public class EntityDataBase
    {
        /// <summary>
        /// 直接扱うときは注意。getから呼べばコピーが出るよ。
        /// </summary>
        public readonly List<Entity> Entities = new List<Entity>();



        Dictionary<string, List<Entity>> EDB = new Dictionary<string, List<Entity>>();
        Dictionary<string, bool> EDBF = new Dictionary<string, bool>();

        Dictionary<string, List<Entity>> CEDB = new Dictionary<string, List<Entity>>();
        Dictionary<Type, List<Entity>> TEDB = new Dictionary<Type, List<Entity>>() { { typeof(Entity), new List<Entity>() } };
        /// <summary>
        /// エンテティをデータベースにぶち込むメソッド。
        /// 一生変わらない特性はここで振り分ける
        /// </summary>
        /// <param name="e"></param>
        /// <param name="add">先頭に入れるならfalse</param>
        /// <returns>既に追加されていなかったか</returns>
        virtual public bool entadd(Entity e, bool add)
        {
            if (!Entities.Contains(e))
            {
                if (add)
                {
                    Entities.Add(e);
                } else
                {
                    Entities.Insert(0, e);
                }
                var a = ARhuri(e);
                if (add)
                {
                    foreach (var b in a)
                    {
                        CEDB[b].Add(e);
                    }
                }
                else
                {
                    foreach (var b in a)
                    {
                        CEDB[b].Insert(0, e);
                    }
                }

                {
                    Type t = e.GetType();
                    {
                        Type pre = t;

                        while (!TEDB.ContainsKey(pre))
                        {
                            TEDB.Add(pre, new List<Entity>());
                            pre = t.BaseType;
                        }
                    }

                    if (add)
                    {
                        foreach (var aa in TEDB.Keys)
                        {
                            if (t == aa || t.IsSubclassOf(aa))
                            {
                                TEDB[aa].Add(e);
                            }
                        }
                    }
                    else
                    {
                        foreach (var aa in TEDB.Keys)
                        {
                            if (t == aa || t.IsSubclassOf(aa))
                            {
                                TEDB[aa].Insert(0, e);
                            }
                        }
                    }

                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// エンテティをデータベースからぶち消す
        /// 一生変わらない特性はここでリストから削除しとく
        /// </summary>
        /// <param name="e"></param>
        /// <returns>既に追加されていなかったか</returns>
        virtual public bool entremove(Entity e)
        {
            if (Entities.Remove(e))
            {
                var a = ARhuri(e);
                foreach (var b in a)
                {
                    CEDB[b].Remove(e);
                }
                {
                    Type t = e.GetType();


                    foreach (var aa in TEDB.Keys)
                    {
                        if (t == aa || t.IsSubclassOf(aa))
                        {
                            TEDB[aa].Remove(e);
                        }
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 追加されるときにCEDBに振り分けるためのメソッド
        /// </summary>
        /// <param name="e">振り分けるエンテティ</param>
        /// <returns>どのCEDBに入れるか</returns>
        virtual protected List<string> ARhuri(Entity e)
        {
            var res = new List<string>();

            return res;

        }
        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        public EntityDataBase()
        {
            setDB();
        }
        /// <summary>
        /// 新しいデータ列を追加する
        /// </summary>
        /// <param name="c">コンスタントのか（毎フレームごとの検査は必要ない奴のこと）</param>
        /// <param name="name">その名前</param>
        protected void add(string name, bool c = false)
        {
            if (c)
            {
                CEDB.Add(name, new List<Entity>());
            }
            else
            {
                EDB.Add(name, new List<Entity>());
                EDBF.Add(name, false);
            }
        }
        /// <summary>
        /// コンストラクタで呼び出されるデータ列セットマン
        /// </summary>
        virtual protected void setDB()
        {
            add("moves");
            add("atarens");
            add("atarerus");
            add("overweights");
        }
        /// <summary>
        /// データ列にリストを登録し、フラグをtrueにする
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="lis">そのリスト</param>
        protected void sets(string name, List<Entity> lis)
        {
            EDB[name] = lis;
            EDBF[name] = true;
        }
        /// <summary>
        /// フレーム処理の時に呼び出す奴
        /// フラグをfalseにするだけ
        /// </summary>
        virtual public void frame()
        {
            var a = new List<string>(EDBF.Keys);
            foreach (var b in a)
            {
                EDBF[b] = false;
            }
        }
        /// <summary>
        /// タイプ別にエンテティを取得する
        /// </summary>
        /// <typeparam name="T">Entity、もしくは継承したクラス</typeparam>
        /// <returns>リストだよ</returns>
        public List<T> getTypeEnts<T>()
            where T : Entity
        {
            var t = typeof(T);
            if (TEDB.ContainsKey(t))
            {
                return new List<T>(TEDB[t].ConvertAll(a => (T)a));
            }
            return new List<T>();
        }
        /// <summary>
        /// タイプ別にエンテティを取得する
        /// </summary>
        /// <param name="T">タイプ</param>
        /// <returns>リストだよ</returns>
        public List<Entity> getTypeEnts(Type T)
        {
            var t = T;
            if (TEDB.ContainsKey(t))
            {
                return new List<Entity>(TEDB[t]);
            }
            return new List<Entity>();
        }
       

        /// <summary>
        /// なにかしら呼び出す
        /// </summary>
        /// <param name="name">なまえentsで全部</param>
        /// <returns>そっちでキャストしてくれ～い</returns>
        virtual public List<Entity> get(string name)
        {
            var res = already(name);
            if (res != null) return res;

            seton(name);


            return already(name);
        }
        /// <summary>
        /// 名前によってデータベーにつける。ここで振り分けとかする
        /// </summary>
        /// <param name="name">名前</param>
        virtual protected void seton(string name)
        {
            if (name == "moves"||name=="overweights"||name=="atarens"||name=="atarerus")
            {
                var wei = new List<Entity>();
                var mov = new List<Entity>();
                var ren = new List<Entity>();
                var reru = new List<Entity>();

                foreach (var a in Entities) 
                {
                    if (a.atariable)
                    {
                        if (a.bif.ovw) wei.Add(a);
                        else mov.Add(a);
                        reru.Add(a);
                    }
                    else 
                    {
                        ren.Add(a);
                    }
                }
                sets("moves", mov);
                sets("overweights", wei);
                sets("atarens", ren);
                sets("atarerus", reru);
            }
        }
        /// <summary>
        /// 既にフラグがtrueの奴はそのまま返す奴
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>フラグがtrueのやつ</returns>
        protected List<Entity> already(string name) 
        {
            if (CEDB.ContainsKey(name)) 
            {
                return new List<Entity>(CEDB[name]);
            }
            if (EDBF[name]) 
            {
                return new List<Entity>(EDB[name]);
            }

            return null;
        }
        

    }
    /// <summary>
    /// エンテティをまとめて物理をしてくれるやーつ
    /// </summary>
    public class EntityManager
    {
        hyojiman hyo;
        /// <summary>
        /// 結び付けられてる表示マン
        /// </summary>
        public hyojiman hyoji { get { return hyo; } }
        /// <summary>
        /// エンテティをまとめてるやつ
        /// </summary>
        protected EntityDataBase EDB = new EntityDataBase();

        /// <summary>
        /// 普通のコンストラクタ
        /// </summary>
        /// <param name="hyou"></param>
        public EntityManager(hyojiman hyou)
        {
            hyo = hyou;
        }

        /// <summary>
        /// エンテティのリスト
        /// </summary>
        public List<Entity> ents { get { return EDB.getTypeEnts<Entity>(); } }
        /// <summary>
        /// 物理的に当たれる中の動くというか重さが限界じゃないやつら
        /// </summary>
        public List<Entity> moves { get { return EDB.get("moves"); } }
        /// <summary>
        /// 物理的に当たれる中の重さが限界に達してる奴ら
        /// </summary>
        public List<Entity> overweights { get { return EDB.get("overweights"); } }
        /// <summary>
        /// 物理的に当たれる奴ら
        /// </summary>
        public List<Entity> atarerus { get { return EDB.get("atarerus"); } }
        /// <summary>
        /// 物理的に当たれないやつら
        /// </summary>
        public List<Entity> atarens { get { return EDB.get("atarens"); } }


        /// <summary>
        /// タイプ別にエンテティを取得する
        /// </summary>
        /// <typeparam name="T">Entity、もしくは継承したクラス</typeparam>
        /// <returns>リストだよ</returns>
        public List<T> getTypeEnts<T>()
            where T : Entity
        {
            return EDB.getTypeEnts<T>();
        }
        /// <summary>
        /// タイプ別にエンテティを取得する
        /// </summary>
        /// <param name="T">タイプ</param>
        /// <returns>リストだよ</returns>
        public List<Entity> getTypeEnts(Type T)
        {

            return EDB.getTypeEnts(T);
        }

        /// <summary>
        /// エンテティのタイプがどうとか返す
        /// </summary>
        /// <param name="T">タイプ</param>
        /// <param name="e">調査対象</param>
        /// <returns>当てはまってら</returns>
        public bool istyped(Type T, Entity e)
        {
            return EDB.getTypeEnts(T).Contains(e);
        }

        /// <summary>
        /// エンテティをマネージャーにぶち込む。基本ENtity.addを呼べ
        /// 表示もしてくれる
        /// </summary>
        /// <param name="e">ぶち込むやつ</param>
        /// <param name="add">先頭に加えたい場合はfalse</param>
        /// <returns>もうぶち込まれてたらfalse</returns>
        internal bool add(Entity e, bool add)
        {

            return EDB.entadd(e, add);
        }
        /// <summary>
        /// エンテティをを削除する。基本ENtity.REmoveを呼べ
        /// </summary>
        /// <param name="e">削除する奴</param>
        /// <returns>そもそも存在していなかったらfalse</returns>
        internal bool remoeve(Entity e)
        {

            return EDB.entremove(e);
        }


        /// <summary>
        /// リセットなんだけどnew ENtityManagerの方が正確かな
        /// </summary>
        public void reset()
        {


            EDB.frame();
            foreach (var a in ents)
            {
                remoeve(a);
            }
            hyo.reset();
            EDB.frame();

        }
        /// <summary>
        /// 当たったよっていうなんかデータ
        /// </summary>
        protected Dictionary<Entity, List<Entity>> atalis = new Dictionary<Entity, List<Entity>>();
        /// <summary>
        /// これがこれに当たったよ！というお知らせ
        /// </summary>
        /// <param name="korega">これが</param>
        /// <param name="koreni">これに</param>
        public void atattao(Entity korega, Entity koreni)
        {
            if (!atalis.ContainsKey(korega)) atalis.Add(korega, new List<Entity>());
            atalis[korega].Add(koreni);
        }
        /// <summary>
        /// これがこれに当たっているのかを知る
        /// </summary>
        /// <param name="korega">これが</param>
        /// <param name="koreni">これに</param>
        /// <returns>当たったの？</returns>
        public bool atattano(Entity korega, Entity koreni)
        {
            if (atalis.ContainsKey(korega)) return atalis[korega].Contains(koreni);
            return false;
        }


        /// <summary>
        /// 反射のなんかデータ
        /// </summary>
        protected Dictionary<Entity, List<Entity>> hansyasu = new Dictionary<Entity, List<Entity>>();

        /// <summary>
        /// 反射処理をおこなったかを記憶させる
        /// </summary>
        /// <param name="korega">これが</param>
        /// <param name="koreni">これに反射した</param>
        public void hansyao(Entity korega, Entity koreni)
        {
            if (!hansyasu.ContainsKey(korega)) hansyasu.Add(korega, new List<Entity>());
            if (!hansyasu[korega].Contains(koreni)) hansyasu[korega].Add(koreni);
        }

        /// <summary>
        /// 反射処理をやったのかを知る
        /// </summary>
        /// <param name="korega">これ</param>
        /// <returns></returns>
        public List<Entity> hansyano(Entity korega)
        {
            if (hansyasu.ContainsKey(korega)) return hansyasu[korega];
            return new List<Entity>();
        }
        /*
        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">クロック時間</param>
        virtual public void frame(float cl )
        {
            int i = 0;
            atalis.Clear();
            hansyasu.Clear();



            EDB.frame();

            var elis = new List<Entity>(ents);
            foreach (var a in elis) a.frame(cl);


           

            bool ren;

            foreach (var a in moves)
            {

                ren = false;


                foreach (var b in atarerus)
                {
                    if (!atattano(a, b) && a.bif.different(b.bif) && (a.Acore.atarun2(a.PAcore, b.Acore, b.PAcore)))
                    {
                        if (a.bif.zuren(a, b))
                        {
                            atattao(a, b);

                            ren = true;

                            hansyao(a, b);
                        }
                    }
                }
                while (ren)
                {
                    ren = false;
                    foreach (var b in atarerus)
                    {
                        if (!atattano(a, b) && a.bif.different(b.bif) && a.Acore.atarun(b.Acore))
                        {
                            if (a.bif.zuren(a, b))
                            {
                                atattao(a, b);

                                ren = true;

                                hansyao(a, b);
                            }
                        }
                    }
                }

            }
            foreach (var a in overweights)
            {
                foreach (var b in moves)
                {
                    if (!atattano(a, b) && a.bif.different(b.bif) && a.Acore.atarun(b.Acore))
                    {
                        if (b.bif.zuren(b,a))
                        {

                        }
                    }
                }
            }
            var cons = new Dictionary<Entity, List<Entity>>(hansyasu);
            var keys = new List<Entity>(cons.Keys);
            foreach (var a in keys)
            {
                if (cons[a] != null)
                {
                    for (i = cons[a].Count - 1; i >= 0; i--)
                    {
                        //  Console.WriteLine(i +"; ;size of awr :" +cons[a].Count);
                        if (a.Acore.atarun(cons[a][i].Acore) || hansyasu[a].Count - 1 == i)
                        {

                            a.bif.hansya(a,cons[a][i]);

                            if (cons.ContainsKey(cons[a][i]))
                            {
                                cons[cons[a][i]].Remove(a);
                            }

                        }
                        else
                        {
                            hansyasu[a].RemoveAt(i);

                        }
                    }
                }
            }




          
            foreach (var a in elis) a.endframe(cl);
           
        }*/
        /// <summary>
        /// すべての物体を重ならないように動かす
        /// </summary>
        /// <param name="es">軽い順</param>
        protected void zurasiall(List<Entity> es)
        {
            int i;


            //  foreach (var a in es) Console.WriteLine(a.bif.wei+" a sa ");
            //  Console.WriteLine(es.Count + " :;asdlgka:p ");
           
            for (i = 0; i < es.Count; i++)
            {

                // if (es[i].bif.ovw) break;
                for (int t = 0; t < es.Count; t++)
                {

                    if (i != t && !es[i].bif.isgrouped(es[t]))
                    {
                        if (es[t].bif.different(es[i].bif))
                        {
                            var kek = Shape.atarun(es[t].Acore, es[t].PAcore, es[i].Acore, es[i].PAcore);
                            //  Console.WriteLine("asfkjaijfia "+kek);
                            if (kek)
                            {
                                //     Console.WriteLine("aslkgapo");
                                var l = es[t].bif.zuren(es[t], es[i], true);

                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
           
           
            for (i = 0; i < es.Count; i++)
            {
                es[i].bif.groupclear();
                es[i].bif.energyConserv(es[i]);
            }

        }
        /// <summary>
        /// すべての物体を反射させる
        /// </summary>
        /// <param name="es">軽い順</param>
        protected void hansyaall(List<Entity>es )
        {
            
            for (int i = 0; i < es.Count; i++)
            {
                if (es[i].bif.ovw) break;
                for (int t = i + 1; t < es.Count; t++)
                {
                    if (es[t].bif.different(es[i].bif))
                    {
                        es[i].bif.setsessyoku(es[i], es[t]);
                    }
                }
            }


            
                for (int i = 0; i < es.Count; i++)
            {

                if (es[i].bif.ovw)
                {
                    break;
                }//	TRACE(_T("%f :asfas: %f\n"), es[i]->vx, es[i]->vy);
                //es[i]->sessyokukaiten(cl);

                for (int t = i + 1; t < es.Count; t++)
                {
                    
                    es[i].bif.SessyokuHansya(es[i], es[t]);

                }
            }


            foreach (var a in es)
            {
                a.bif.resetsessyokus();

               //  Console.WriteLine(a.Acore.getCenter().ToString() + " daaa");
            }
        }
        /// <summary>
        /// すべての物体をずらし、物理系のほかのもする
        /// </summary>
        protected void buturiall(float cl)
        {
            var es = atarerus;

            var e = new supersort<Entity>();
            foreach (var a in es)
            {
                e.add(a, a.bif.wei);
                a.bif.setEnergyPoint(a);
            }
            e.sort(false);
            es = e.getresult();
            zurasiall(es);
            hansyaall(es);
        } 
        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">クロック時間</param>
       virtual public void frame(float cl)
        {
            EDB.frame();
            var es = ents;
            foreach(var a in new List<Entity>(es))
            {
                //Console.WriteLine(a.Acore.getCenter().ToString() + " Qaaa");
                a.frame(cl);
               // Console.WriteLine(a.Acore.getCenter().ToString() + " Qaaa");
            }
           // Console.WriteLine("framed!");
           
                //Console.WriteLine(aslfka+"asl:kgaso "+aslfka);
             buturiall(cl);
            // Console.WriteLine(aslfka+"asl:kgaso QQQQQQQQQQQ"+aslfka);

            //Console.WriteLine("buturied!");

            foreach (var a in new List<Entity>(es))
            {
             //  Console.WriteLine(a.Acore.getCenter().ToString() + " daaa");
            
                a.endframe(cl);

                //  Console.WriteLine(a.Acore.getCenter().ToString() + " daaa");
            }
         //   Console.WriteLine("endframed!");
            // Console.WriteLine(" sakdaso @@@");
        }

    }



    
    

}
