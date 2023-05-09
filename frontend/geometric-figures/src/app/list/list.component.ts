import { Component, ViewChild } from '@angular/core';
import { Figure } from '../services/figure';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort, Sort } from '@angular/material/sort';
import { FiguresService } from '../services/figures.service';
import { PageEvent } from '@angular/material/paginator';
import { FiguresResponse } from '../services/figures-response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss'],
})
export class ListComponent {
  public figures: Figure[];
  public dataSource: MatTableDataSource<Figure> = new MatTableDataSource([]);
  public displayedColumns: string[] = [
    'id',
    'name',
    'area',
    'perimeter',
    'actions',
  ];
  public sortField = 'id';
  public isAscending = false;
  public pageSize = 10;
  public pageIndex = 0;
  public totalFigures = 0;
  public searchValue = '';
  public searchEnteredText = '';

  @ViewChild(MatSort) sort: MatSort;

  constructor(private figuresService: FiguresService, private router: Router) {}

  ngOnInit() {
    this.getFigures();
    this.dataSource.sort = this.sort;
  }

  handlePageEvent(e: PageEvent) {
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;
    this.getFigures();
  }

  sortData(sort: Sort) {
    this.sortField = sort.active;
    this.isAscending = sort.direction === 'asc';
    this.getFigures();
  }

  getFigures() {
    this.figuresService
      .get(
        this.sortField,
        this.isAscending,
        this.pageSize,
        this.pageIndex + 1,
        this.searchValue
      )
      .subscribe((result: FiguresResponse) => {
        this.figures = result.figures;
        this.dataSource.data = this.figures;
        this.totalFigures = result.totalFigures;
      });
  }

  search() {
    if (this.searchValue !== this.searchEnteredText) {
      this.searchValue = this.searchEnteredText;
      this.pageIndex = 0;
      this.getFigures();
    }
  }

  delete(figure: Figure) {
    this.figuresService.delete(figure).subscribe(() => {
      if (this.figures.length === 1) {
        this.pageIndex =
          this.pageIndex > 0 ? this.pageIndex - 1 : this.pageIndex;
      }
      this.getFigures();
    });
  }

  edit(figure: Figure) {
    this.router.navigate(['edit/' + figure.id]);
  }
}
