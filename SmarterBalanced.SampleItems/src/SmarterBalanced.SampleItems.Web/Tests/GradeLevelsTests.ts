import * as GradeLevels from '../Scripts/GradeLevels';

it('caseToString for grade 3', () => {
    const grade = GradeLevels.caseToString(GradeLevels.GradeLevels.Grade3);
    expect(grade).toBe("Grade 3");
});

it('caseToString for high school', () => {
    const grade = GradeLevels.caseToString(GradeLevels.GradeLevels.High);
    expect(grade).toBe("High");
});