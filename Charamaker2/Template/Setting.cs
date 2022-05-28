using Charamaker2;
using Charamaker2.Character;
using Charamaker2.input;
using Charamaker2.Shapes;
using GameSet1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template
{
    class Setting : Scene
    {
        character inami; serif serif;
        float inati = 0;
        int tate = -1;
        List<selectbox> selects = new List<selectbox>();
        List<selectbox> yajis = new List<selectbox>();
        selectbox exit;
        public Setting(SceneManager s, Scene next) : base(s)
        {
            this.next = next;
        }
        public override void start()
        {

            base.start();
            new picture(0, 0, -1000000, hyo.ww, hyo.wh, 0, 0, 0, false, 0.7f, "def", new Dictionary<string, string> { { "def", @"window\setting" } }).add(hyo);
            float h = hyo.wh / 10;
            float w = hyo.ww * 0.5f;
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 100, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "exit&save"), 50, 50);
                sss.add();
                exit = sss;


            }
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 300, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 300, h, 150, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "master volume"), hyo.ww / 2, 0.5f * h);
                selects.Add(sss);
                sss.add();

                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, Math.PI)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, true, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, ".", kyotyousuru: false), hyo.ww / 2 - (maxwid / 2 + 30) - 30 - 30, 0.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, 0)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, "〇", kyotyousuru: false), hyo.ww / 2 + (maxwid / 2 + 30) + 50 + 10, 0.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }

            }
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 300, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 300, h, 150, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "SE volume"), hyo.ww / 2, 1.5f * h);
                selects.Add(sss);
                sss.add();

                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, Math.PI)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, true, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, ".", kyotyousuru: false), hyo.ww / 2 - (maxwid / 2 + 30) - 30 - 30, 1.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, 0)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, "〇", kyotyousuru: false), hyo.ww / 2 + (maxwid / 2 + 30) + 50 + 10, 1.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
            }
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 300, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 300, h, 150, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "BGM volume"), hyo.ww / 2, 2.5f * h);
                selects.Add(sss);
                sss.add();

                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, Math.PI)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, true, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, ".", kyotyousuru: false), hyo.ww / 2 - (maxwid / 2 + 30) - 30 - 30, 2.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, 0)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, "〇", kyotyousuru: false), hyo.ww / 2 + (maxwid / 2 + 30) + 50 + 10, 2.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
            }
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 300, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 300, h, 150, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "image quality"), hyo.ww / 2, 3.5f * h);
                selects.Add(sss);
                sss.add();
                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, Math.PI)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, true, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, ".", kyotyousuru: false), hyo.ww / 2 - (maxwid / 2 + 30) - 30 - 30, 3.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
                {
                    var ssss = new selectbox(hyo, new Triangle(0, 0, 100, h, 0.5f, 0)
                        , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 100, h, 50, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\yaji" }, { "selected", @"window\yajiselected" } }), new List<setu>()))
                        , new message(0, 0, h / 2, 30, 15, 0, -1, "〇", kyotyousuru: false), hyo.ww / 2 + (maxwid / 2 + 30) + 50 + 10, 3.5f * h);
                    yajis.Add(ssss);

                    ssss.add();
                }
            }
            {
                var sss = new selectbox(hyo, new Rectangle(0, 0, 300, h, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, 300, h, 150, h / 2, 0, false, 1, "def", new Dictionary<string, string> { { "def", @"window\def" }, { "selected", @"window\selected" } }), new List<setu>()))
                    , new message(0, 0, h / 2, 30, 15, 0, -1, "key config"), hyo.ww / 2, 4.5f * h);
                selects.Add(sss);
                sss.add();
            }


            //   fileman.playbgm("setting");
            inamiset();
            inamimove("taiki", "inamikaere", 222);
        }
        void inamiset()
        {
            if (inami != null)
            {
                var pre = inami;
                inami = fileman.loadcharacter("inami", 2f);
                inami.copykakudo(pre);
                pre.sinu(hyo);
            }
            else
            {
                inami = fileman.loadcharacter("inami", 2f);
            }
            inami.setcxy(hyo.ww / 2, hyo.wh * 0.99f, inami.tx, inami.h);
            inami.resethyoji(hyo);
        }
        void inamimove(string move, string sel, float time, float size = 0.5f)
        {
            if (inati <= 0)
            {
                inati = time;
                if (serif != null) serif.sinu(hyo);
                inamiset();
                inami.addmotion(fileman.ldmotion(@"inami\" + move));
                if (sel != "") inami.addmotion(fileman.ldmotion(@"inami\syaberu"));
                serif = new serif(hyo, 60, inami, 40, 2, FP.GT(sel), size, 1);
            }
        }

        public override void frame(inputin i, float cl)
        {
            base.frame(i, cl);
            inami.frame(1);
            if (inati > 0) inati -= 1;
            selesoroe();
            input(i);
        }
        public void input(inputin i)
        {

            if (i.ok(MouseButtons.Left, itype.down))
            {
                if (exit.on(i))
                {
                    end();
                }
                if (selects[4].on(i))
                {
                    fileman.playoto("kettei");
                    new KeyConfig(sm, new Setting(sm, next)).start();

                }
                for (int j = 0; j < 8; j++)
                {
                    if (yajis[j].on(i))
                    {
                        int sl = 10;
                        if (j % 2 == 0) sl *= -1;
                        fileman.playoto("scroll1");
                        suchan(sl, j / 2);

                    }
                }
            }

            if (i.ok(Keys.Escape, itype.down))
            {
                end();
            }


            if (i.ok(Keys.D, itype.ing))
            {
                suchan(1);
                fileman.playoto("scroll1");
            }

            if (i.ok(Keys.A, itype.ing))
            {
                suchan(-1);
                fileman.playoto("scroll1");
            }
            if (i.ok(Keys.W, itype.down))
            {
                settatesu(-1);
                fileman.playoto("scroll2");
            }

            if (i.ok(Keys.S, itype.down))
            {
                settatesu(+1);
                fileman.playoto("scroll2");
            }
            if (i.ok(Keys.Space, itype.down))
            {
                if (tate == 4)
                {
                    fileman.playoto("kettei");
                    new KeyConfig(sm, new Setting(sm, next)).start();
                }
            }
        }

        void suchan(int sl, int to = -1)
        {

            if (to == -1) to = tate;
            switch (to)
            {
                case 0:
                    SD.S.mvol += 0.01f * sl;
                    
                    if (fileman.percentin(33))
                    {
                        inamimove("attention", "inamioto1", 222);
                    }
                    else if (fileman.percentin(50))
                    {

                        inamimove("kikoeru", "inamioto2", 222);
                    }
                    else
                    {

                        inamimove("kikoerumaji", "inamioto3", 222);
                    }
                    break;
                case 1:

                    SD.S.kvol += 0.01f * sl;
                  
                    if (fileman.percentin(33))
                    {
                        inamimove("attention", "inamikouka1", 222);
                    }
                    else if (fileman.percentin(50))
                    {

                        inamimove("kikoeru", "inamikouka2", 222);
                    }
                    else
                    {

                        inamimove("niou", "inamikouka3", 222);
                    }
                    break;
                case 2:

                    SD.S.bvol += 0.01f * sl;
                    //ここ！大事！ fileman.playbgm("setting",true);
                    if (fileman.percentin(33))
                    {
                        inamimove("attention", "inamibgm1", 222);
                    }
                    else if (fileman.percentin(50))
                    {

                        inamimove("kikoeru", "inamibgm2", 222);
                    }
                    else
                    {

                        inamimove("yareyare", "inamibgm3", 222);
                    }
                    break;
                case 3:

                    SD.S.gsit += 0.01f * sl;
                    if (fileman.percentin(50))
                    {
                        inamimove("attention", "inamigasitu1", 222);
                    }
                    else if (fileman.percentin(50))
                    {

                        inamimove("niou", "inamigasitu2", 222);
                    }
                    else
                    {

                        inamimove("attention", "inamigasitu3", 222);
                    }
                    break;
                default:
                    break;
            }
        }
        public void settatesu(int slide)
        {
            //  Console.WriteLine(tate+"asfawr");
            if (0 <= tate + slide && tate + slide < selects.Count)
            {

                tate += slide;
            }
            else if (tate < 0) tate = 0;


            foreach (var a in yajis) a.chara.core.p.texname = "def";
            for (int i = 0; i < selects.Count; i++)
            {
                if (i == tate)
                {
                    selects[i].chara.core.p.texname = "selected";
                    if (i < 4)
                    {
                        yajis[i * 2].chara.core.p.texname = "selected";
                        yajis[i * 2 + 1].chara.core.p.texname = "selected";
                    }
                }
                else
                {
                    selects[i].chara.core.p.texname = "def";
                }
            }
        }
        public override void end()
        {
            fileman.playoto("kettei");
            base.end();

        }
        float maxwid = 500;
        void selesoroe()
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        selects[i].box.w = SD.S.mvol * maxwid;
                        selects[i].chara.core.p.w = SD.S.mvol * maxwid;
                        selects[i].chara.core.p.tx = SD.S.mvol * maxwid / 2;
                        break;
                    case 1:
                        selects[i].box.w = SD.S.kvol * maxwid;
                        selects[i].chara.core.p.w = SD.S.kvol * maxwid;
                        selects[i].chara.core.p.tx = SD.S.kvol * maxwid / 2;
                        break;
                    case 2:
                        selects[i].box.w = SD.S.bvol * maxwid;
                        selects[i].chara.core.p.w = SD.S.bvol * maxwid;
                        selects[i].chara.core.p.tx = SD.S.bvol * maxwid / 2;
                        break;
                    case 3:
                        selects[i].box.w = SD.S.gsit * maxwid;
                        selects[i].chara.core.p.w = SD.S.gsit * maxwid;
                        selects[i].chara.core.p.tx = SD.S.gsit * maxwid / 2;
                        break;
                    default:
                        break;
                }
                selects[i].setcxy(hyo.ww / 2, selects[i].box.getty());
                selects[i].soroeru();
            }
        }
    }
    public class KeyConfig : Scene
    {
        character inami; serif serif;
        float inati = 0;
        int domcou = 0;
        int selected = -1;
        List<selectbox> buttons = new List<selectbox>();
        public KeyConfig(SceneManager s, Scene next) : base(s)
        {
            this.next = next;
        }
        float boxsize = 50;
        int mojisize = 20;
        public override void start()
        {

            base.start();
            new picture(0, 0, -1000000, hyo.ww, hyo.wh, 0, 0, 0, false, 0.7f, "def", new Dictionary<string, string> { { "def", @"window\setting" } }).add(hyo);

            inamiset();
            inamimove("attention", "inamikeykaeri", 60);
            keyhyoji();
            for (int i = 0; i < SD.S.converts.Count(); i++)
            {

                buttons.Add(new selectbox(hyo, new Rectangle(0, 0, boxsize * 0.9f, boxsize * 0.9f, 0)
                    , new character(0, 0, 0, 0, 0, 0, 0, new setu("core", 0, 0, new picture(0, 0, 0, boxsize * 0.9f, boxsize * 0.9f, boxsize * 0.9f / 2, boxsize * 0.9f / 2, 0, true, 1, "def", new Dictionary<string, string> { { "def", @"window\def" } }), new List<setu>()))
                    , new message(0, 0, 1, 1, 0, 0, 0, ""), hyo.ww * (0.02f + (i % 4) / 4f), hyo.ww * 0.02f + boxsize * ((int)(i / 4))));
                buttons[i].add();

            }
        }
        public override void frame(inputin i, float cl)
        {

            base.frame(i, cl);
            for (int ii = 0; ii < SD.S.converts.Count(); ii++)
            {
                if (selected == ii)
                {
                    buttons[ii].text = new message(buttons[ii].box.gettx() - mojisize / 2, buttons[ii].box.getty() - mojisize / 2, mojisize, 100, 0, 0, 1, SD.S.converts[ii].getString(), 1, 0, 0, true);
                    buttons[ii].text.add(hyo);
                }
                else
                {
                    buttons[ii].text = new message(buttons[ii].box.gettx() - mojisize / 2, buttons[ii].box.getty() - mojisize / 2, mojisize, 100, 0, 0, 1, SD.S.converts[ii].getString(), 0, 0, 0, false);
                    buttons[ii].text.add(hyo);
                }
            }
            inami.frame(1);
            if (inati > 0) inati -= 1;
            input(i);
        }
        public override void end()
        {
            fileman.playoto("kettei");
            base.end();
        }

        public void input(inputin i)
        {

            if (i.ok(itype.down))
            {
                if (selected == -1)
                {
                    for (int t = 0; t < buttons.Count; t++)
                    {
                        if (buttons[t].on(i))
                        {
                            selected = t;
                            keyhyoji();
                            fileman.playoto("kettei");

                            inamimove("nani", "inamikeywhat", 20);

                            break;
                        }

                    }
                    if (selected == -1)
                    {
                        domcou += 1;
                        fileman.playoto("scroll1");
                        if (domcou > 5) end();
                    }
                    else { domcou = 0; }
                }
                else
                {
                    var k = inputin.raw.getdownkey();
                    var m = inputin.raw.getdownbutton();

                    bool succesed = false;
                    if (k.Count > 0)
                    {
                        foreach (var a in SD.S.converts)
                        {
                            if (a.getin(k.Last()))
                            {
                                if (SD.S.converts[selected].KI != Keys.None)
                                {
                                    a.changein(SD.S.converts[selected].KI);
                                }
                                else
                                {
                                    a.changein(SD.S.converts[selected].MI);
                                }
                            }
                        }
                        succesed = SD.S.converts[selected].changein(k.Last());

                    }
                    else if (m.Count > 0)
                    {
                        foreach (var a in SD.S.converts)
                        {
                            if (a.getin(m.Last()))
                            {
                                if (SD.S.converts[selected].KI != Keys.None)
                                {
                                    a.changein(SD.S.converts[selected].KI);
                                }
                                else
                                {
                                    a.changein(SD.S.converts[selected].MI);
                                }
                            }
                        }

                        succesed = SD.S.converts[selected].changein(m.Last());
                    }
                    else
                    {
                        selected = -1;
                        if (fileman.percentin(50)) inamimove("niou", "inamikeyno1", 20);
                        else inamimove("yareyare", "inamikeyno2", 20);
                        keyhyoji();
                        return;
                    }

                    if (fileman.percentin(30))
                    {
                        inamimove("attention", "inamikeykaeriend", 20);
                    }
                    else if (succesed)
                    {
                        inamimove("douda", "inamikeydeteke", 20);

                    }
                    else
                    {
                        inamimove("yareyare", "inamikeysemete", 20);

                    }
                    selected = -1;
                    fileman.playoto("kettei");
                    keyhyoji();

                }
            }
        }
        void inamiset()
        {
            if (inami != null)
            {
                var pre = inami;
                inami = fileman.loadcharacter("inami", 2f);
                inami.copykakudo(pre);
                pre.sinu(hyo);
            }
            else
            {
                inami = fileman.loadcharacter("inami", 2f);
            }
            inami.setcxy(hyo.ww / 2, hyo.wh * 0.99f, inami.tx, inami.h);
            inami.resethyoji(hyo);
        }
        void inamimove(string move, string sel, float time, float size = 0.5f)
        {
            if (inati <= 0)
            {
                inati = time;
                if (serif != null) serif.sinu(hyo);
                inamiset();
                inami.addmotion(fileman.ldmotion(@"inami\" + move));
                if (sel != "") inami.addmotion(fileman.ldmotion(@"inami\syaberu"));
                serif = new serif(hyo, 60, inami, 40, 2, FP.GT(sel), size, 1);
            }
        }
        List<message> keyhyos = new List<message>();
        void keyhyoji()
        {
            for (int i = keyhyos.Count() - 1; i >= 0; i--)
            {
                keyhyos[i].remove(hyo);
                keyhyos.RemoveAt(i);
            }
            for (int i = 0; i < SD.S.converts.Count(); i++)
            {

                var a = SD.S.converts[i];
                float R = 0;
                if (i == selected) R = 1;
                keyhyos.Add(new message(hyo.ww * (0.03f + (i % 4) / 4f), hyo.ww * 0.01f + ((int)(i / 4)) * boxsize, mojisize, 1000, 0, 0, -1, a.getString(), R));

            }
        }
    }
    public class selectbox
    {
        public Shape box;
        public character chara;
        public message text;
        hyojiman hyo;
        public selectbox(hyojiman h, Shape s, character c, message m, float tx, float ty)
        {
            hyo = h;
            box = s;
            chara = c;
            text = m;
            setcxy(tx, ty);
        }
        public void add()
        {
            if (chara != null)
            {
                chara.resethyoji(hyo);
            }
            else
            {
                //  hyo.shapes.Add(box);
            }
            text.add(hyo);
        }
        public void frame(float cl = 1)
        {
            if (chara != null)
                chara.frame(cl);
        }
        public void setcxy(float x, float y, float tx = 0.5f, float ty = 0.5f)
        {
            box.setcxy(x, y, box.w * tx, box.h * ty);
            soroeru();
        }
        public void soroeru()
        {
            text.x = box.gettx(); text.y = box.getty() - text.SIZE / 2;
            if (chara != null) chara.setcxy(box.getcx(box.w / 2, box.h / 2), box.getcy(box.w / 2, box.h / 2), chara.w / 2, chara.h / 2);
        }
        public void remove()
        {
            if (chara != null)
            {
                chara.sinu(hyo);
            }
            else
            {
                //hyo.shapes.Remove(box);
            }
            text.remove(hyo);
        }
        public void slide(float f) { box.x += f; soroeru(); }
        public bool on(inputin i)
        {
            return box.onhani(i.x, i.y);
        }

    }
}
