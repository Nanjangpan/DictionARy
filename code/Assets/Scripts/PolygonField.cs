using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

using System.Linq;
using System.Text;


static class Constants //상수 값들 모음
{
    public const double char_distance1 = 0.27; //두 단어카드의 거리(좌우)
    public const double char_distance2 = 0.35; //두 단어카드의 거리(위아래)
}






public class PolygonField : MonoBehaviour
{

    public GameObject[] go_raw;
    //public Text[] go_text_angle;
    //public Text[] go_text_distance;
    //채연
    public Text[] go_text_letter;
    public Text perimeter_text;
    public Text area_text;
    public Text type_text;


    private Vector2[] go_points;
    //private Text[] go_points_text_a;
    private Text[] go_points_text_d;
    //채연
    private Text[] go_points_text_l;
    private GameObject[] go_n;

    private LineRenderer lineRenderer;
    private MeshFilter filter;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        filter = gameObject.GetComponent<MeshFilter>();
    }

    void Update()
    {
        getAllAvailablePoints();
        //draw();
        string output = vowel_func();
        calculation(output);
        allocate_char();
    }

    char[] cho = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    string[] cho_eng = { "giyeok", "ggiyeok", "nieun", "digeut", "ddigeut", "rieul", "mieum", "bieup", "bbieup", "shiot", "sshiot", "ieung", "jieut", "jjieut", "chieut", "kieuk", "tieut", "pieup", "hieung" };

    char[] jung = { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ',
                            'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };

    string[] jung_eng = { "ah", "ya", "eo", "yeo", "o", "yo", "u", "yu", "ue", "i" };

    char[] jong = { ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ',
                            'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    string[] jong_eng = { " ", "giyeok", "ggiyeok", "gs", "nieun", "nj", "nh", "digeut", "rieul", "rg", "rm", "rb", "rs", "rt", "rp", "rh", "mieum", "bieup", "bs", "shiot", "sshiot", "ieung", "jieut", "chieut", "kieuk", "tieut", "pieup", "hieung" };



    private string vowel_func()
    {
        List<int> vowel = new List<int>();
        List<float> pos_x = new List<float>();
        List<float> pos_y = new List<float>();
        string[] jung_eng = { "ah", "ya", "eo", "yeo", "o", "yo", "u", "yu", "ue", "i" };
        for (int i = 0; i < go_raw.Length; i++)
        {
            if (go_raw[i].GetComponent<MeshRenderer>().enabled)
            {
                if (jung_eng.Contains(go_raw[i].name))
                {
                    int num = i - 14;
                    vowel.Add(num);
                    pos_x.Add(go_raw[i].transform.position.x);
                    pos_y.Add(go_raw[i].transform.position.y);
                }
            }
        }
        if (true) //merge vowel
        {
            //ㅐ = ㅏ + ㅣ = 0 + 9
            if (vowel.Contains(0) && vowel.Contains(9))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 0)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 9)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(10);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅒ = ㅑ + ㅣ = 1 + 9
            if (vowel.Contains(1) && vowel.Contains(9))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 1)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 9)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(11);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅔ = ㅓ + ㅣ = 2 + 9
            if (vowel.Contains(2) && vowel.Contains(9))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 2)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 9)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(12);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅖ = ㅕ + ㅣ = 3 + 9
            if (vowel.Contains(3) && vowel.Contains(9))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 3)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 9)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(13);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }

            //ㅘ = ㅗ + ㅏ = 4 + 0
            if (vowel.Contains(0) && vowel.Contains(4))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 0)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 4)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && b_x < a_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(14);
                            pos_x.Add(b_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅚ = ㅗ + ㅣ = 4 + 9
            if (vowel.Contains(9) && vowel.Contains(4))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 9)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 4)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && b_x < a_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(15);
                            pos_x.Add(b_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅝ = ㅜ + ㅓ = 2 + 6
            if (vowel.Contains(2) && vowel.Contains(6))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 2)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 6)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && b_x < a_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(16);
                            pos_x.Add(b_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅟ = ㅜ + ㅣ = 6 + 9
            if (vowel.Contains(9) && vowel.Contains(6))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 9)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 6)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && b_x < a_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(17);
                            pos_x.Add(b_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅢ = ㅡ + ㅣ = 8 + 9
            if (vowel.Contains(9) && vowel.Contains(8))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 9)
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 8)
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.20 && b_x < a_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(18);
                            pos_x.Add(b_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅙ 4 + 10
            if (vowel.Contains(4) && vowel.Contains(10))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 4)//ㅗ
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 10)//ㅐ
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.27 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(19);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
            //ㅞ 6 + 12
            if (vowel.Contains(6) && vowel.Contains(12))
            {
                List<int> A = new List<int>();
                List<int> B = new List<int>();
                List<int> M_A = new List<int>();
                List<int> M_B = new List<int>();
                for (int i = 0; i < vowel.Count; i++)
                {
                    if (vowel[i] == 6)//ㅜ
                    {
                        A.Add(i);
                    }
                    if (vowel[i] == 12)//ㅔ
                    {
                        B.Add(i);
                    }
                }
                float a_x, a_y, b_x, b_y;
                double d;
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < B.Count; j++)
                    {
                        a_x = pos_x[A[i]];
                        a_y = pos_y[A[i]];
                        b_x = pos_x[B[j]];
                        b_y = pos_y[B[j]];
                        d = distance(a_x, a_y, b_x, b_y);
                        if (d < 0.27 && a_x < b_x && Math.Abs(a_y - b_y) < 0.02)
                        {
                            M_A.Add(A[i]);
                            M_B.Add(B[j]);
                            vowel.Add(20);
                            pos_x.Add(a_x);
                            pos_y.Add((a_y + b_y) / 2);
                        }
                    }
                }
                List<int> target = new List<int>();
                M_A.AddRange(M_B);
                M_A.Sort();
                for (int i = M_A.Count - 1; i >= 0; i--)
                {
                    vowel.RemoveAt(M_A[i]);
                    pos_x.RemoveAt(M_A[i]);
                    pos_y.RemoveAt(M_A[i]);
                }
            }
        }
        for (int i = 0; i < vowel.Count; i++)
        {
            /*
             * 0
             * 10
             * 1
             * 11
             * 2
             * 12
             * 3
             * 13
             * 4
             * 14
             * 19
             * 15
             * 5
             * 6
             * 16
             * 20
             * 17
             * 7
             * 8
             * 18
             * 9
             */
            switch (vowel[i])
            {
                case 0:
                    vowel[i] = 0;
                    break;
                case 10:
                    vowel[i] = 1;
                    break;
                case 1:
                    vowel[i] = 2;
                    break;
                case 11:
                    vowel[i] = 3;
                    break;
                case 2:
                    vowel[i] = 4;
                    break;
                case 12:
                    vowel[i] = 5;
                    break;
                case 3:
                    vowel[i] = 6;
                    break;
                case 13:
                    vowel[i] = 7;
                    break;
                case 4:
                    vowel[i] = 8;
                    break;
                case 14:
                    vowel[i] = 9;
                    break;
                case 19:
                    vowel[i] = 10;
                    break;
                case 15:
                    vowel[i] = 11;
                    break;
                case 5:
                    vowel[i] = 12;
                    break;
                case 6:
                    vowel[i] = 13;
                    break;
                case 16:
                    vowel[i] = 14;
                    break;
                case 20:
                    vowel[i] = 15;
                    break;
                case 17:
                    vowel[i] = 16;
                    break;
                case 7:
                    vowel[i] = 17;
                    break;
                case 8:
                    vowel[i] = 18;
                    break;
                case 18:
                    vowel[i] = 19;
                    break;
                case 9:
                    vowel[i] = 20;
                    break;
            }
        }
        string combine_word = make_word(vowel, pos_x, pos_y);
        return combine_word;
    }




    public bool check_left_bottom(int idx) // True면 left_bottom , False면 top_bottom, 0,1,2,3,4,5,6,7,20
    {
        bool check = false;
        if (idx <= 7 || idx == 20)
        {
            check = true;
        }
        return check;
    }

    
    private void allocate_char()
    {
        for (int i = 0; i < go_raw.Length; i++)
        {
            //모음 위주로 단어 공간 저장
            if (go_raw[i].GetComponent<MeshRenderer>().enabled)
            {
                //print(go_raw[i].name);
                if (go_text_letter[i].text != "") //비어 있지 않을 때에만 초기화(왜냐하면 합친 단어를 넣을 때 문제가 생김)
                {
                    //print(go_raw[i].name);
                    go_text_letter[i].text = go_raw[i].name;
                }
            }
        }
    }
    

    private string make_word(List<int> vowel, List<float> pos_x, List<float> pos_y)
    {
        int vowel_length = vowel.Count;
        int one_vowel;
        float x, y; // 모음 x, y좌표
        string cho_front, cho_top, cho_bottom;
        float cho_top_x = 0, cho_top_y = 0; //x,y 좌표 값
        float cho_bottom_x =0 , cho_bottom_y = 0; //x,y 좌표 값
        string second_front, second_top, second_bottom; //합치기 위해 만든 것

        int unity_cho_f = 0, unity_cho_t = 0, unity_cho_b = 0;

        List<char> Made = new List<char>();
        List<float> Made_x = new List<float>();

        for (int i = 0; i < vowel_length; i++)
        {
            one_vowel = vowel[i];
            x = pos_x[i];
            y = pos_y[i];
            cho_front = "";
            cho_top = "";
            cho_bottom = "";
            second_front = "";
            second_top = "";
            second_bottom = "";


            for (int j = 0; j < go_raw.Length; j++)
            {
                if (go_raw[j].GetComponent<MeshRenderer>().enabled)
                {
                    //go_text_letter[i].text = go_raw[i].name;
                    if (cho_eng.Contains(go_raw[j].name)) //초성인지 검사
                    {
                        //distances
                        double dv = distance(x, y, go_raw[j].transform.position.x, go_raw[j].transform.position.y);
                        if (check_left_bottom(one_vowel)) //모음에서 자음이 왼쪽, 밑에 오는 경우
                        {
                            if (dv < Constants.char_distance1)
                            {
                                if (x > go_raw[j].transform.position.x && Math.Abs(y - go_raw[j].transform.position.y) < 0.1)
                                { //자음이 모음 왼쪽에 있지만 y좌표는 거의 차이가 없을때
                                    cho_front = go_raw[j].name;
                                    unity_cho_f = j;
                                }
                                else if (y > go_raw[j].transform.position.y && Math.Abs(x - go_raw[j].transform.position.x) < 0.1)
                                { //자음이 모음 밑에 있지만 x좌표는 거의 차이가 없을때
                                    cho_bottom = go_raw[j].name;
                                    cho_bottom_x = go_raw[j].transform.position.x;
                                    cho_bottom_y = go_raw[j].transform.position.y;
                                }
                            }
                            if (dv < 2*Constants.char_distance1 && dv>Constants.char_distance1) //두번째 붙이는 것은 더 여유 있게 준다.
                            {
                                if (x > go_raw[j].transform.position.x && Math.Abs(y - go_raw[j].transform.position.y) < 0.1)
                                { //자음이 모음 왼쪽에 있지만 y좌표는 거의 차이가 없을때
                                    second_front = go_raw[j].name;
                                }
                                else if (Math.Abs(y - go_raw[j].transform.position.y) < Constants.char_distance1)
                                { //자음이 모음 밑에 있고, x좌표는 cho_bottom_x 보다 작고 y좌표는 cho_bottom_y랑 차이가 없을 때
                                    if(cho_bottom_x > go_raw[j].transform.position.x && Math.Abs(cho_bottom_y - go_raw[j].transform.position.y) < 0.15) 
                                    { 
                                        second_bottom = go_raw[j].name;
                                    }
                                }
                            }

                        }
                        else //모음에서 자음이 위, 밑에 오는 경우
                        {
                            if (dv < Constants.char_distance2)
                            {
                                if (y < go_raw[j].transform.position.y && Math.Abs(x - go_raw[j].transform.position.x) < 0.1)
                                { //자음이 모음 위쪽에 있지만 x좌표는 거의 차이가 없을때

                                    cho_top = go_raw[j].name;
                                    cho_top_x = go_raw[j].transform.position.x;
                                    cho_top_y = go_raw[j].transform.position.y;
                                    unity_cho_t = j;
                                }
                                else if (y > go_raw[j].transform.position.y && Math.Abs(x - go_raw[j].transform.position.x) < 0.15)
                                { //자음이 모음 밑에 있지만 x좌표는 조금 차이가 날 경우
                                    cho_bottom = go_raw[j].name;
                                    cho_bottom_x = go_raw[j].transform.position.x;
                                    cho_bottom_y = go_raw[j].transform.position.y;
                                }
                            }
                            if (dv < 2 * Constants.char_distance1 && dv > Constants.char_distance1) //두번째 붙이는 것은 더 여유 있게 준다.
                            {
                                if (Math.Abs(cho_top_x - go_raw[j].transform.position.x) < Constants.char_distance1 && Math.Abs(cho_top_y - go_raw[j].transform.position.y) < 0.1)
                                { //두개의 top 차이가 저기 안에서 나고 y좌표 차이가 거의 없을 때
                                    second_top = go_raw[j].name;
                                }
                                else if (Math.Abs(y - go_raw[j].transform.position.y) < 2* Constants.char_distance2)
                                { //자음이 모음 밑에 있고, x좌표는 cho_bottom_x 보다 작고 y좌표는 cho_bottom_y랑 차이가 없을 때
                                    if (cho_bottom_x > go_raw[j].transform.position.x && Math.Abs(cho_bottom_y - go_raw[j].transform.position.y) < 0.5)
                                    {
                                        second_bottom = go_raw[j].name;
                                    }
                                }
                            }
                        }
                    }
                }

            }

            /*
            print("cho_front");
            print(cho_front);
            print("cho_bottom");
            print(cho_bottom);
            print("cho_top");
            print(cho_top);
            */
            //print(second_front);
            print("1");
            print(cho_bottom);
            print("2");
            print(second_bottom);

            // 앞에 오는 자음 합치는 함수 ㄲ, ㄸ, ㅃ, ㅉ
            if(cho_front!= "" && second_front!= "") 
            {
                if(cho_front == second_front) // 같을 경우에만 합쳐진다.
                {
                    switch(cho_front)
                    {
                        case "giyeok":
                            cho_front = "ggiyeok";
                            break;
                        case "digeut":
                            cho_front = "ddigeut";
                            break;
                        case "bieup":
                            cho_front = "bbieup";
                            break;
                        case "jieut":
                            cho_front = "jjieut";
                            break;
                    }
                }
            }

            // 위에 오는 자음 합치는 함수 ㄲ, ㄸ, ㅃ, ㅉ
            if (cho_top != "" && second_top != "")
            {
                if (cho_top == second_top) // 같을 경우에만 합쳐진다.
                {
                    switch (cho_top)
                    {
                        case "giyeok":
                            cho_top = "ggiyeok";
                            break;
                        case "digeut":
                            cho_top = "ddigeut";
                            break;
                        case "bieup":
                            cho_top = "bbieup";
                            break;
                        case "jieut":
                            cho_top = "jjieut";
                            break;
                    }
                }
            }

            //cho_bottom이 모음 밑에 있는것 second_bottom이 모음 밑 왼쪽에 있는 것
            // 밑에 오는 자음 합치는 함수 ㄲ, ㄳ, ㄵ, ㄶ, ㄺ, ㄻ, ㄼ, ㄽ, ㄾ, ㅀ, ㅄ, ㅆ
            if (cho_bottom != "" && second_bottom != "")
            { 
              switch (second_bottom)
              {
                case "giyeok":
                    if(cho_bottom == "giyeok")
                    {
                        cho_bottom = "ggiyeok";
                    }
                    else if (cho_bottom == "shiot")
                    {
                        cho_bottom = "gs";
                    }
                    break;
                case "nieun":
                    if(cho_bottom == "jieut")
                    {
                        cho_bottom = "nj";
                    }
                    else if (cho_bottom == "hieung")
                    {
                        cho_bottom = "nh";
                    }
                    break;
                case "rieul":
                    if(cho_bottom == "giyeok")
                    {
                        cho_bottom = "rg";
                    }
                    else if (cho_bottom == "mieum")
                    {
                        cho_bottom = "rm";
                    }
                    else if (cho_bottom == "bieup")
                    {
                        cho_bottom = "rb";
                    }
                    else if (cho_bottom == "shiot")
                    {
                        cho_bottom = "rs";
                    }
                    else if (cho_bottom == "tieut")
                    {
                        cho_bottom = "rt";
                    }
                    else if (cho_bottom == "hieung")
                    {
                        cho_bottom = "rh";
                    }
                    break;
                case "bieup":
                    if(cho_bottom == "shiot")
                    {
                        cho_bottom = "bs";
                    }
                    break;
                case "shiot":
                    if(cho_bottom == "shiot")
                    {
                        cho_bottom = "sshiot";
                    }
                    break;
              }
            }

            //합치는 구간
            if (cho_front != "") //왼쪽이 빈공간이 아닐때만 하기
            {
                int cho_i = 0, jung_i = one_vowel, jong_i = 0;

                for (int l = 0; l < 19; l++)
                {
                    if (cho_eng[l] == cho_front)
                    {
                        cho_i = l;
                        break;
                    }
                }

                for (int l = 0; l < 28; l++)
                {
                    if (jong_eng[l] == cho_bottom)
                    {
                        jong_i = l;
                        break;
                    }
                }

                int uniValue = (cho_i * 21 * 28) + (jung_i * 28) + (jong_i) + 0xAC00;

                go_text_letter[unity_cho_f].text = char.ToString((char)uniValue); // 유니티에 보여주기 위한 코드

                //print((char)uniValue);
                Made.Add((char)uniValue);
                Made_x.Add(x);
            }
            else if (cho_top != "") //윗글자가 빈공간이 아닐 때
            {
                int cho_i = 0, jung_i = one_vowel, jong_i = 0;

                for (int l = 0; l < 19; l++)
                {
                    if (cho_eng[l] == cho_top)
                    {
                        cho_i = l;
                        break;
                    }
                }

                for (int l = 0; l < 28; l++)
                {
                    if (jong_eng[l] == cho_bottom)
                    {
                        jong_i = l;
                        break;
                    }
                }

                int uniValue = (cho_i * 21 * 28) + (jung_i * 28) + (jong_i) + 0xAC00;


                go_text_letter[unity_cho_t].text = char.ToString((char)uniValue); // 유니티에 보여주기 위한 코드

                //print((char)uniValue);
                Made.Add((char)uniValue);
                Made_x.Add(x);
            }
            else //위쪽, 왼쪽 빈공간이면 오류 발생
            {
                print("단어가 완성되지 않습니다");
            }


        }
        List<int> arg = new List<int>();
        List<int> tmp = new List<int>();
        for (int i = 0; i < Made_x.Count; i++)
        {
            tmp.Add(i);
        }
        while (Made_x.Count != 0)
        {
            float min = Made_x.Min();
            int target = -1;
            int i;
            for (i = 0; i < Made_x.Count; i++)
            {
                if (Made_x[i] == min)
                {
                    target = tmp[i];
                    break;
                }
            }
            arg.Add(target);
            Made_x.RemoveAt(i);
            tmp.RemoveAt(i);
        }
        string Ans = "";
        for (int i = 0; i < Made.Count; i++)
        {
            Ans += Made[arg[i]].ToString();
        }
        if (Ans != "")
        {
            //print(Ans);
        }
        return Ans;
    }


    private void getAllAvailablePoints()
    {
        // Create new Vector2 and Text Lists
        List<Vector2> vertices2DList = new List<Vector2>();
        //List<Text> textAList = new List<Text>();
        //List<Text> textDList = new List<Text>();
        List<Text> textLList = new List<Text>();
        List<GameObject> oList = new List<GameObject>();

        // Fill lists if availble
        for (int i = 0; i < go_raw.Length; i++)
        {
            if (go_raw[i] != null)
            {
                if (go_raw[i].GetComponent<MeshRenderer>().enabled)
                {
                    //go_text_angle[i].enabled = true;
                    //go_text_distance[i].enabled = true;
                    //채연
                    go_text_letter[i].enabled = true;

                    vertices2DList.Add(new Vector2(go_raw[i].transform.position.x, go_raw[i].transform.position.y));
                    //textAList.Add(go_text_angle[i]);
                    //textDList.Add(go_text_distance[i]);
                    //채연
                    textLList.Add(go_text_letter[i]);
                    oList.Add(go_raw[i]);
                }
                else
                {
                    //go_text_angle[i].enabled = false;
                    //go_text_distance[i].enabled = false;
                    go_text_letter[i].enabled = false;
                }
            }
        }

        // Convert to array
        //go_points_text_a = textAList.ToArray();
        //go_points_text_d = textDList.ToArray();
        go_points_text_l = textLList.ToArray();
        go_points = vertices2DList.ToArray();
        go_n = oList.ToArray();
    }

    private void calculation(string output)
    {
        // Perimeter varible
        double p = 0;
        // Area varible
        double area = 0;
        // Type varible
        int n = 0;

        // Loop all points
        for (int i = 0; i < go_points.Length; i++)
        {
            //         For test neighbors
            //         int x0, x1, x2;

            // Point before
            Vector2 v0;
            if ((i - 1) >= 0)
            {
                v0 = go_points[i - 1];                      // x0 = i - 1;
            }
            else
            {
                v0 = go_points[go_points.Length - 1];       // x0 = go_points.Length - 1;
            }

            // Point now
            Vector2 v1 = go_points[i];                      // x1 = i;

            // Point after
            Vector2 v2;
            if ((i + 1) < go_points.Length)
            {
                v2 = go_points[i + 1];                      // x2 = i + 1;
            }
            else
            {
                v2 = go_points[0];                          // x2 = 0;
            }

            // triangular distances
            double dv0 = distance(v0.x, v0.y, v1.x, v1.y); // v0 & v1
            double dv1 = distance(v1.x, v1.y, v2.x, v2.y); // v1 & v2
            double dv2 = distance(v0.x, v0.y, v2.x, v2.y); // v0 & v2

            // Perimeter      
            p += dv1;

            // Area
            double temp_area = (v1.x * v2.y) - (v1.y * v2.x);
            area += temp_area;

            // Type
            n++;

            // Angle    
            // Set point angle ∠v0v1v2
            //double a = angle(dv0, dv1, dv2, v0.x, v0.y, v1.x, v1.y, v2.x, v2.y);
            //go_points_text_a[i].text = Math.Round(a) + "°";
            //   test neighbors 
            //   go_points_text_a [i].text = i + "# " + x0 + " " + x1 + " " + x2;

            // Distance
            // Set distance position
            Vector2 mp = midPoint(v1.x, v1.y, v2.x, v2.y);
            //go_points_text_d[i].transform.parent.position = new Vector3(mp.x, mp.y, go_points_text_d[i].transform.parent.position.z);

            // Set distance angle
            // double az = angle_zero(v1.x, v1.y, v2.x, v2.y);
            //go_points_text_d[i].transform.parent.eulerAngles = new Vector3(go_points_text_d[i].transform.parent.eulerAngles.x, go_points_text_d[i].transform.parent.eulerAngles.y, (float)az);

            // Set "point" and "point after" distance
            //go_points_text_d[i].text = Math.Round(dv1, 2) + "";

            // Set letter    
            go_points_text_l[i].text = i + "";

        }

        // Set type
        type_text.text = "word: " + output;

    }

    // Distance between two points (Pitagor theory)
    private double distance(float x1, float y1, float x2, float y2)
    {
        float a = Math.Abs(x1 - x2);
        float b = Math.Abs(y1 - y2);
        double c = Math.Sqrt(a * a + b * b);

        return c;
    }

    // Angle between two lines(three points) anticlockwise
    private double angle(double i1, double i2, double i3, float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
    {
        double k = ((i2 * i2) + (i1 * i1) - (i3 * i3)) / (2 * i1 * i2);
        double d = Math.Acos(k) * (180 / Math.PI);

        double dd = direction(p1x, p1y, p2x, p2y, p3x, p3y);

        if (dd > 0)
        {
            d = 360 - d;
        }

        return d;
    }
    private double direction(float x1, float y1, float x2, float y2, float x3, float y3)
    {
        double d = ((x2 - x1) * (y3 - y1)) - ((y2 - y1) * (x3 - x1));
        return d;
    }

    // Middle Point betwwen two points
    private Vector2 midPoint(float x1, float y1, float x2, float y2)
    {
        float x = (x1 + x2) / 2;
        float y = (y1 + y2) / 2;
        return new Vector2(x, y);
    }

    // Zero way angle betwwen two points
    private double angle_zero(float x1, float y1, float x2, float y2)
    {
        double xDiff = x2 - x1;
        double yDiff = y2 - y1;
        double d = Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
        return d;
    }
}