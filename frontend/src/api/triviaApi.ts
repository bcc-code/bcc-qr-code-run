import baseUrl from "./baseApi";

export async function getTrivia() {
    const result = await fetch(`${baseUrl}/trivia`);
    return await result.json() as FunFact[];
}

export interface FunFact {
    title: string,
    content: string
}
