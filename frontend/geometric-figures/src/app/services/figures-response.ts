import { Figure } from './figure';

export class FiguresResponse {
  constructor(public figures: Figure[], public totalFigures: number) {}
}
