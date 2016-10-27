enum GradeLevels {
    NA = 0,
    Grade3 = 1 << 0,
    Grade4 = 1 << 1,
    Grade5 = 1 << 2,
    Grade6 = 1 << 3,
    Grade7 = 1 << 4,
    Grade8 = 1 << 5,
    Grade9 = 1 << 6,
    Grade10 = 1 << 7,
    Grade11 = 1 << 8,
    Grade12 = 1 << 9,
    Elementary = Grade3 | Grade4 | Grade5,
    Middle = Grade6 | Grade7 | Grade8,
    High = Grade9 | Grade10 | Grade11 | Grade12,
    All = Elementary | Middle | High
}