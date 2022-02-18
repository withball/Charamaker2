using Charamaker2;
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
        /// <param name="name">その名前</param>
        protected void add(string name)
        {
            EDB.Add(name, new List<Entity>());
            EDBF.Add(name, false);
        }
        /// <summary>
        /// コンストラクタで呼び出されるデータ列セットマン
        /// </summary>
        virtual protected void setDB()
        {
            add("ents");
            add("moves");
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
        public void frame()
        {
            var a = new List<string>(EDBF.Keys);
            foreach (var b in a)
            {
                EDBF[b] = false;
            }
        }
        /// <summary>
        /// なにかしら呼び出す
        /// </summary>
        /// <param name="name">なまえentsで全部</param>
        /// <returns>そっちでキャストしてくれ～い</returns>
        virtual public List<Entity> get(string name="ents")
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
            if (name == "ents") 
            {
                sets("ents", new List<Entity>(Entities));
            }
            if (name == "moves"||name=="overweights")
            {
                var wei = new List<Entity>();
                var mov = new List<Entity>();
                foreach (var a in Entities) 
                {
                    if (a.bif.ovw) wei.Add(a);
                    else mov.Add(a);
                }
                sets("moves", mov);
                sets("overweights", wei);
            }
        }
        /// <summary>
        /// 既にフラグがtrueの奴はそのまま返す奴
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>フラグがtrueのやつ</returns>
        protected List<Entity> already(string name) 
        {
            if (EDBF[name]) 
            {
                return EDB[name];
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
        protected EntityDataBase EDB=new EntityDataBase();

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
        public List<Entity> ents { get { return EDB.get("ents"); } }
        /// <summary>
        /// 動くというか重さが限界じゃないやつら
        /// </summary>
        public List<Entity> moves { get { return EDB.get("moves"); } }
        /// <summary>
        /// 重さが限界に達してる奴ら
        /// </summary>
        public List<Entity> overweights { get { return EDB.get("overweights"); } }

        /// <summary>
        /// エンテティをマネージャーにぶち込む。基本ENtity.addを呼べ
        /// 表示もしてくれる
        /// </summary>
        /// <param name="e">ぶち込むやつ</param>
        /// <returns>もうぶち込まれてたらfalse</returns>
        public bool add(Entity e)
        {
            if (!EDB.Entities.Contains(e)) 
            {
                EDB.Entities.Add(e);

                return true;
            }
            return false;
        }
        /// <summary>
        /// エンテティをを削除する。基本ENtity.REmoveを呼べ
        /// </summary>
        /// <param name="e">削除する奴</param>
        /// <returns>そもそも存在していなかったらfalse</returns>
        public bool remoeve(Entity e)
        {

            return EDB.Entities.Remove(e);
        }


        /// <summary>
        /// リセットなんだけどnew ENtityManagerの方が正確かな
        /// </summary>
        public void reset()
        {
           
            
            EDB.frame();
            foreach (var a in ents)
            {
                a.remove();
            }
            hyo.reset();
            EDB.frame();
        
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~EntityManager()
        {
            reset();
        }
        Dictionary<Entity, List<Entity>> atalis = new Dictionary<Entity, List<Entity>>();
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
       


        Dictionary<Entity, List<Entity>> hansyasu = new Dictionary<Entity, List<Entity>>();

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

        /// <summary>
        /// フレーム処理
        /// </summary>
        /// <param name="cl">クロック時間</param>
        public void frame(float cl )
        {
            int i = 0;
            atalis.Clear();
            hansyasu.Clear();



            EDB.frame();

            for (i = ents.Count - 1; i >= 0; i--) ents[i].frame(cl);


           

            bool ren;
            
            foreach (var a in moves)
            {

                ren = false;

                if (a.atariable)
                {
                    foreach (var b in ents)
                    {
                        if (!atattano(a, b) && b.atariable && a.bif.different(b.bif) && (a.Acore.atarun2(a.PAcore, b.Acore, b.PAcore)))
                        {
                            if (a.bif.zuren(a,b))
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
                        foreach (var b in ents)
                        {
                            if (!atattano(a, b) && b.atariable && a.bif.different(b.bif) && a.Acore.atarun(b.Acore))
                            {
                                if (a.bif.zuren(a,b))
                                {
                                    atattao(a, b);

                                    ren = true;

                                    hansyao(a, b);
                                }
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




          
            foreach (var a in ents) a.setpab();
           
        }
    
    }

}
