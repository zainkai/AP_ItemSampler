
class ItemPageManager {
    accessibilityMap: { [key: string]: string};

    constructor(private ajaxURL: string) {
        this.accessibilityMap = { };
        this.bindEvents();
    }

    bindEvents(): void {
        this.bindAccessibilityChange();
        this.bindAccessibilityBtns();
    }

    toISAAP(): string {
        const values = Object.keys(this.accessibilityMap).map(k => this.accessibilityMap[k]);
        return values.join(";");
    }

    setAccessibilityMapItem(item: HTMLSelectElement): void {
        const key = item.id;
        const val = item.options[item.selectedIndex].value;
        this.accessibilityMap[key] = val;
    }

    onAccessibilityChange(eventData: Event): void {
        this.setAccessibilityMapItem(eventData.target as HTMLSelectElement);        
    }

    updateISAAP(): void {
        const iframe = document.getElementById("itemviewer-iframe") as HTMLIFrameElement;
        if (iframe != null) {
            const urlParts = iframe.src.split("isaap");
            iframe.src = urlParts[0] + "isaap=" + this.toISAAP();
        }
        else {
            console.error("unable to access iframe")
        }
    }

    bindAccessibilityChange(): void {
        const accElements = document.getElementsByClassName("accessibilityResources");
        for (let i = 0; i < accElements.length; i++) {
            let element = accElements[i] as HTMLSelectElement;
            element.addEventListener("change", e => this.onAccessibilityChange(e));
            this.setAccessibilityMapItem(element);
        }
    }

    bindAccessibilityBtns(): void {
        const updateButton = document.getElementById("btn-accessibility-update") as HTMLElement;
        updateButton.addEventListener("click", e => this.updateISAAP());

        const resetButton = document.getElementById("btn-accessibility-reset") as HTMLElement;
        resetButton.addEventListener("click", e => this.resetToGlobalAccessibility());
    }

    replaceAccessibilityContent(id: string, newContent: string): void {
        const modalContent = document.getElementById(id) as HTMLElement;
        modalContent.innerHTML = newContent;

        this.bindEvents();
        this.updateISAAP();
    }

    resetToGlobalAccessibility() {
        $.ajax({
            url: this.ajaxURL,
            type: "GET",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            success: result => {
                var id = "accessibilityContent";
                this.replaceAccessibilityContent(id, result);
            },
            error: function (result) {
                console.error("Error resetting item accessibility");
            }
        });
    }
}

function initializeItemAccessibility(ajaxURL: string) {
    document.addEventListener("DOMContentLoaded", function () { new ItemPageManager(ajaxURL); });
}
