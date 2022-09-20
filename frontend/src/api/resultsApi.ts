import baseUrl from "./baseApi";

export async function getResults() {
    const result = await fetch(`${baseUrl}/results`);
    return await result.json() as Result[];
}

export async function myChurch() {
    const result = await fetch(`${baseUrl}/results/mychurch`, {
        credentials: "include"
    });
    return await result.json() as ChurchResult;
}

export interface Result {
    church: string,
    points: number
} 

export interface ChurchResult {
    churchName: string,
    teams: number,
    score: number,
    secretsFound: number,
    timeSpent: string
}