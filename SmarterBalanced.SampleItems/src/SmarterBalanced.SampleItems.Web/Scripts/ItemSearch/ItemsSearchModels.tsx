export interface SubjectClaims {
    [subject: string]: { text: string; value: string }[];
}

export interface Loading {
    kind: "loading";
}

export interface Success<T> {
    kind: "success";
    content: T;
}

export interface Failure {
    kind: "failure";
}

export interface Reloading<T> {
    kind: "reloading";
    content: T;
}

/** Represents the state of an asynchronously obtained resource at a particular time. */
export type Resource<T> = Loading | Success<T> | Reloading<T> | Failure

export interface InteractionType {
    code: string;
    label: string;
}

export interface Subject {
    code: string;
    label: string;
    claims: Claim[];
    interactionTypeCodes: string[];
}

export interface Claim {
    code: string;
    label: string;
    targets: Target[];
}


export interface Target {
    name: string;
    nameHash: number;
}
