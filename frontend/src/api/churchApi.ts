import baseUrl from "./baseApi";

export async function getChurches() {
    const result = await fetch(`${baseUrl}/churches`);
    return await result.json() as string[];
}