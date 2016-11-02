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

namespace GradeLevels {
    export function toString(grades: GradeLevels) {
        // If a GradeLevels value exactly matches one of the declared enum members, it can be obtained by indexing into the enum.
        const gradeString = GradeLevels[grades];
        if (gradeString) {
            return gradeString;
        }

        let gradeStrings: string[] = []
        // If a GradeLevels value has several grades in it that don't match Elementary/Middle/High, we have to go through and add them up.
        for (let i = 0; i < 10; i++) {
            if ((grades & 1 << i) === 1 << i) {
                gradeStrings.push(GradeLevels[1 << i]);
            }
        }
        return gradeStrings.join(", ");
    }
}