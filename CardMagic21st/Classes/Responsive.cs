﻿//////////////////////////////////////////////
//////////////////////////////////////////////
// Coded by Mehedi Shams Rony ////////////////
// September, October 2016 ///////////////////
//////////////////////////////////////////////
//////////////////////////////////////////////
using System;
using System.Drawing;

namespace CardMagic21st.Classes
{
    class Responsive
    {
        const float WIDTH_AT_DESIGN_TIME = 1920;
        const float HEIGHT_AT_DESIGN_TIME = 1080;
        Rectangle Resolution;
        float WidthMultiplicationFactor;
        float HeightMultiplicationFactor;

        public Responsive(Rectangle ResolutionParam)
        {
            WidthMultiplicationFactor = 1;
            Resolution = ResolutionParam;
        }

        public void SetMultiplicationFactor()
        {
            WidthMultiplicationFactor = Resolution.Width / WIDTH_AT_DESIGN_TIME;
            HeightMultiplicationFactor = Resolution.Height / HEIGHT_AT_DESIGN_TIME;
        }

        public int GetMetrics(int ComponentValue)
        {
            return (int)(Math.Floor(ComponentValue * WidthMultiplicationFactor));
        }

        public int GetMetrics(int ComponentValue, string Direction)
        {
            if (Direction.Equals("Width") || Direction.Equals("Left"))
                return (int)(Math.Floor(ComponentValue * WidthMultiplicationFactor));
            else if (Direction.Equals("Height") || Direction.Equals("Top"))
                return (int)(Math.Floor(ComponentValue * HeightMultiplicationFactor));
            return 1;
        }
    }
}
