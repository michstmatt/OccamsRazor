import { Round } from "./round";

export interface Question{
    gameId: number;
    text: string;
    category: string;
    round: Round;
    number: number;
    answerText: string;
}