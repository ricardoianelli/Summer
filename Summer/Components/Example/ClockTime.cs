﻿namespace Summer.Components.Example;

public record ClockTime(int Hour, int Minute, int Second)
{
    public override string ToString()
    {
        return $"{Hour}:{Minute}:{Second}";
    }
}