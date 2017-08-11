import { GradeLevels, caseToString } from '../Scripts/GradeLevels';

describe('caseToString function', () => {
   test('should correctly convert enums to strings for grade school', () => {
        expect(caseToString(GradeLevels.Grade3)).toBe('Grade 3');
   });

    test('should correctly convert enums to strings for high school', () => {
        expect(caseToString(GradeLevels.High)).toBe('High');
    });
});