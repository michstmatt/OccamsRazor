import { Round } from "./round";
import { GameState } from "./game-state";
export interface GameMetadata {
    gameId: number;
    name: string;
    currentRound: Round;
    currentQuestion: number;
    state: GameState;
    isMultipleChoice: boolean;
    seed: number;
}
