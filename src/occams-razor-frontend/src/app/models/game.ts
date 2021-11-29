import { GameMetadata } from "./game-metadata";
import { Question } from "./question";

export interface Game {
    metadata: GameMetadata;
    questions: Question[];
}
