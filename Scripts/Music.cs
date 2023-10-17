using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    //397182473
    public int rule;
    public int seed;

    public int probability;

    public int steps;
    public int maxTime;

    public int[,] grid;
    public int[,] melody;

    public bool[] channels;

    public AudioSource[] instruments;
    public int[] instrumentMapping;
    private float[] beforeInstruments;

    public float tempo;
    public bool generate;

    private int currNote = 0;
    private float time = 0;

    private System.Random random;

    void Start()
    {
        grid = new int[maxTime, steps];
        melody = new int[steps, maxTime];

        beforeInstruments = new float[instruments.Length];

        for(int i = 0; i < instruments.Length; i ++)
        {
            beforeInstruments[i] = 1f;
        }

        random = new System.Random(seed != -1 ? seed : (int)DateTime.Now.Ticks);

        Generate();
    }

    void Update()
    {
        // if(generate)
        // {
        //     Start();
        //     Generate();
        // }

        time = time + Time.deltaTime;

        if(time > tempo)
        {
            time = 0f;

            currNote ++;

            for(int i = steps - 1; i  > -1; i --)
            {
                if(!channels[i]) continue;

                if(melody[i, currNote] == 1)
                {
                    instruments[instrumentMapping[i]].Play();
                }
                else
                {
                    if(instruments[instrumentMapping[i]].volume != 0 && beforeInstruments[instrumentMapping[i]] == 0f)
                    {

                    }
                    else
                    {
                        instruments[instrumentMapping[i]].Stop();
                    }
                }
            }

            for(int i = 0; i < instruments.Length; i ++)
            {
                beforeInstruments[i] = instruments[i].volume;

                Debug.Log(instruments[i].volume);
            }
        }
    }

    private int GetGrid(int time, int i)
    {
        if(time < 0) time = grid.GetLength(0) - 1 + time;
        if(i < 0) i = grid.GetLength(1) - 1 + i;

        if(time >= grid.GetLength(0)) time -= grid.GetLength(0);
        if(i >= grid.GetLength(1)) i -= grid.GetLength(1);

        if(time >= 0 && i >= 0 && time < grid.GetLength(0) && i < grid.GetLength(1)) return grid[time, i];
        
        return 0;
    }

    private void Generate()
    {
        for(int i = 0; i < grid.GetLength(1); i ++)
        {
            grid[0, i] = random.Next(0, 100) > probability ? 1 : 0;
        }

        string binaryrule = "000" + Convert.ToString(rule, 2);

        for(int time = 0; time < grid.GetLength(0) - 1; time ++)
        {
            for(int i = 0; i < grid.GetLength(1); i ++)
            {
                int currCase = 0;

                for(int j = 0; j < 5; j ++)
                {
                    int currFactor = GetGrid(time, i + j - 2) * (int) Mathf.Pow(2, 4 - j);
                    currCase += currFactor;
                }

                grid[time + 1, i] = (int) 1 - (binaryrule[currCase] - '0');
            }
        }

        for(int i = 0; i < grid.GetLength(1); i ++)
        {
            if(!channels[i]) continue;

            string line = "";

            for(int j = 0; j < grid.GetLength(0); j ++)
            {
                line += (grid[j, i] == 1 ? "1" : " ");
            }

            line = line.Replace(" 1 ", "   ");

            for(int j = 0; j < grid.GetLength(0); j ++)
            {
                melody[i,j] = line[j] - '0';
            }
        }
    }
}
