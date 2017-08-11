import * as React from 'react';
import * as renderer from 'react-test-renderer';
import { ItemsSearch, ItemsSearchClient } from '../Scripts/ItemsSearch';
import { ItemCardViewModel } from '../Scripts/ItemCard';
import { GradeLevels } from '../scripts/GradeLevels';

describe('ItemsSearch', () => {
    test('should display the correct number of cards', () => {
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
        };

        const client: ItemsSearchClient = {
            itemsSearch: (params, onSuccess, onError) => {
                process.nextTick(onSuccess([viewModel]));
            }
        };

        const itemsSearch = renderer.create(<ItemsSearch.ISComponent
            interactionTypes = {[]}
            subjects = {[]}
            apiClient = {client} />
        );
        
        const tree = itemsSearch.toJSON();
        expect(tree).toMatchSnapshot();
    });
});