﻿import baseUrl from "./baseApi";
import {Team} from "./teamApi";

export async function scanCode(data:string): Promise<QrCodeResult|string> {
    const result = await fetch(`${baseUrl}/scan`, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        credentials: "include",
        body: JSON.stringify({
            "data": data,
        })
    });
    
    if(result.ok) {
        return (await result.json()) as QrCodeResult
    }
        
    else return await result.json() as string
}

export interface QrCodeResult {
    points: number,
    team: Team,
    funFact: FunFact
}

interface FunFact {
    title: string,
    content: string
}
    
