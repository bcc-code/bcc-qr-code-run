import baseUrl from "./baseApi";

export async function getResults() {
    const result = await fetch(`${baseUrl}/results`);
    return await result.json() as ChurchResult[];
}

export async function myChurch() {
    const result = await fetch(`${baseUrl}/results/mychurch`, {
        credentials: "include"
    });
    return await result.json() as ChurchResult;
}

export interface ChurchResult {
    church: string,
    country: string,
    teams: number,
    timeSpent: string,
    registrations: number,
    participants: number,
    participation: number,
    points: number,
    averagePoints: number,
    score: number,
    secretsFound: number,
    
}