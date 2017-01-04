
namespace ItemDetails {

    export class ItemDetailsManager {

        constructor(url: string, itemDigest: any) {
            initializeItemAccessibility(url);
            initializeAboutItem(itemDigest);
        }

    }

}
