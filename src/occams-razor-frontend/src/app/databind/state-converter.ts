import { Pipe, PipeTransform } from "@angular/core";
import { GameState } from "../models/game-state";
@Pipe({name: 'StateConvert'})
export class StateConverter implements PipeTransform {
    stringMap: Map<GameState, string> = new Map([
        [GameState.Created, "Created"],
        [GameState.Playing, "Question"],
        [GameState.Results, "Results"],
        [GameState.PreQuestion, "Waiting For Next Question"],
        [GameState.PostQuestion, "Answer"],
    ]);

    transform(value: GameState):string {
        return this.stringMap.get(value) ?? "";
    }

}

