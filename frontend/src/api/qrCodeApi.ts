import baseUrl from "./baseApi";
import {Team} from "./teamApi";

export async function scanCode(): Promise<QrCodeResult|number> {
    const result = await fetch(`${baseUrl}/scan`, {
        method: "POST",
        credentials: "include"
    });
    
    if(result.ok) {
        return await result.json()
    }
        
    else return result.status
}

interface QrCodeResult {
    team: Team,
    funFact: FunFact
}

interface FunFact {
    title: string,
    content: string
}
    
