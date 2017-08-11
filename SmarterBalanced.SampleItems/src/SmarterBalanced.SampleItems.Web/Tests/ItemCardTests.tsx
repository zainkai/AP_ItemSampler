import * as React from 'react';
import * as renderer from 'react-test-renderer';
import { ItemCard, ItemCardViewModel } from '../Scripts/ItemCard';
import { GradeLevels } from '../scripts/GradeLevels';

describe('ItemCard', () => {
    test('should display the correct information', () => {
        const viewModel: ItemCardViewModel = {
            bankKey: 1,
            itemKey: 1,
            title: "Deciding What to Eat 101",
            grade: GradeLevels.Grade3,
            gradeLabel: "Grade 3",
            subjectCode: "LNCH",
            subjectLabel: "LNCH/eating",
            claimCode: "LNCH1",
            claimLabel: "Reading",
            targetHash: "1234",
            targetShortName: "Key Details",
            interactionTypeCode: "MC",
            interactionTypeLabel: "Multiple Choice"
        }

        const itemCard = renderer.create(
            <ItemCard {...viewModel} key={Math.random()} />
        );
        const tree = itemCard.toJSON();
        expect(tree).toMatchSnapshot();
    });
});