import { Player } from "./player";
import { Round} from "./round";

export interface PlayerAnswer{
    player: Player;
    questionNumber: number;
    round: Round;
    gameId: number;
    pointsAwarded: number;
    answerText: string;
}