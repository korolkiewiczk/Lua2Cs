using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaToCs;
using LuaToCs.Utils;



using System;



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _intCon = 2;
	public const int _realCon = 3;
	public const int _stringCon = 4;
	public const int _stringCon2 = 5;
	public const int _break = 6;
	public const int _do = 7;
	public const int _else = 8;
	public const int _elseif = 9;
	public const int _false = 10;
	public const int _for = 11;
	public const int _function = 12;
	public const int _if = 13;
	public const int _in = 14;
	public const int _local = 15;
	public const int _nil = 16;
	public const int _return = 17;
	public const int _repeat = 18;
	public const int _true = 19;
	public const int _while = 20;
	public const int _and = 21;
	public const int _andassgn = 22;
	public const int _assgn = 23;
	public const int _colon = 24;
	public const int _comma = 25;
	public const int _dec = 26;
	public const int _divassgn = 27;
	public const int _dot = 28;
	public const int _dblcolon = 29;
	public const int _eq = 30;
	public const int _gt = 31;
	public const int _gteq = 32;
	public const int _inc = 33;
	public const int _lbrace = 34;
	public const int _lbrack = 35;
	public const int _lpar = 36;
	public const int _lshassgn = 37;
	public const int _lt = 38;
	public const int _lteq = 39;
	public const int _ltlt = 40;
	public const int _minus = 41;
	public const int _minusassgn = 42;
	public const int _modassgn = 43;
	public const int _neq = 44;
	public const int _not = 45;
	public const int _orassgn = 46;
	public const int _plus = 47;
	public const int _plusassgn = 48;
	public const int _question = 49;
	public const int _rbrace = 50;
	public const int _rbrack = 51;
	public const int _rpar = 52;
	public const int _scolon = 53;
	public const int _tilde = 54;
	public const int _tilde2 = 55;
	public const int _times = 56;
	public const int _timesassgn = 57;
	public const int _xorassgn = 58;
	public const int maxT = 74;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public Env env;

public string inputFileName;
public string fileName;

/*--------------------------------------------------------------------------------*/
/*---------------------------- auxiliary methods ------------------------*/
/*--------------------------------------------------------------------------------*/

void Error (string s) {
  if (errDist >= minErrDist) errors.SemErr(la.line, la.col, s);
  errDist = 0;
}

// Return the n-th token after the current lookahead token
Token Peek (int n) {
  scanner.ResetPeek();
  Token x = la;
  while (n > 0) { x = scanner.Peek(); n--; }
  return x;
}

//ident "("
bool IsNotLPar()
{
    var notLPar=Peek(1).kind != _lpar;
	return la.kind==_ident && notLPar;
}

/*--------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------------*/
enum TypeKind {simple, array, pointer, @void}
/*----------------------------- token sets -------------------------------*/

const int maxTerminals = 160;  // set size

static BitArray NewSet(params int[] values) {
  BitArray a = new BitArray(maxTerminals);
  foreach (int x in values) a[x] = true;
  return a;
}

static BitArray
  unaryHead    = NewSet(_plus, _minus, _not, _tilde, _times, _inc, _dec, _and),
  assgnOps     = NewSet(_assgn);
  


	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Lua() {
		env = new Env(inputFileName, fileName);
		
		Operand st; 
		while (StartOf(1)) {
			Chunk(out st);
		}
	}

	void Chunk(out Operand st) {
		st=null; 
		while (StartOf(2)) {
			Statement(out st);
			if (la.kind == 53) {
				Get();
			}
		}
		if (la.kind == 6 || la.kind == 17) {
			LastStatement(out st);
			if (la.kind == 53) {
				Get();
			}
		}
	}

	void Statement(out Operand st) {
		Operand op1=null,op2=null,op3=null; Operand st1=null,st2=null; st = null; 
		if (la.kind == 7) {
			Get();
			Block(out op1);
			Expect(59);
		} else if (la.kind == 20) {
			Get();
			Expression(out op1);
			env.codeGen.While(op1); 
			Expect(7);
			Block(out st1);
			Expect(59);
		} else if (la.kind == 18) {
			Get();
			env.codeGen.Do(); 
			Block(out st1);
			Expect(60);
			Expression(out op1);
			env.codeGen.WhileNeg(op1); 
		} else if (la.kind == 13) {
			Get();
			Expression(out op1);
			Expect(61);
			env.codeGen.If(op1); 
			Block(out st1);
			while (la.kind == 9) {
				Get();
				Expression(out op2);
				env.codeGen.Else(); env.codeGen.If(op2); 
				Expect(61);
				Block(out st2);
			}
			if (la.kind == 8) {
				Get();
				env.codeGen.Else(); 
				Block(out st2);
			}
			Expect(59);
		} else if (la.kind == 11) {
			Get();
			NameList(out op1);
			if (la.kind == 23) {
				Get();
				Expression(out op2);
				Expect(25);
				Expression(out op3);
				if (la.kind == 25) {
					Get();
					Expression(out st1);
				}
				env.codeGen.For(op1, op2, op3, st1); 
				Expect(7);
				Block(out st2);
				Expect(59);
			} else if (la.kind == 14) {
				Get();
				ExprList(out op2);
				env.codeGen.ForEach(op1, op2); 
				Expect(7);
				Block(out st1);
				Expect(59);
			} else SynErr(75);
		} else if (la.kind == 12) {
			Get();
			FuncName(out op1);
			FuncBody(op1);
		} else if (la.kind == 15) {
			Get();
			if (la.kind == 12) {
				Get();
				Name(out op1);
				FuncBody(op1);
			} else if (la.kind == 1) {
				NameList(out op1);
				if (la.kind == 23) {
					Get();
					if (StartOf(3)) {
						ExprList(out op2);
						if (env.codeGen.IsScoped) { env.codeGen.Emit(ListOfOperands.ToOperand(op1, op2, true)); env.codeGen.End(); } else { env.AddFieldWithInitializer(op1, op2); } 
					} else if (la.kind == 62) {
						Get();
						if (la.kind == 36) {
							Get();
						}
						if (la.kind == 4) {
							Get();
						} else if (la.kind == 5) {
							Get();
						} else SynErr(76);
						if (la.kind == 52) {
							Get();
						}
						env.AddField(op1.ToString()); 
					} else SynErr(77);
				}
			} else SynErr(78);
		} else if (la.kind == 62) {
			Get();
			if (la.kind == 4) {
				Get();
			} else if (la.kind == 5) {
				Get();
			} else SynErr(79);
		} else if (StartOf(3)) {
			ExprList(out op1);
			env.codeGen.Emit(op1); 
			env.codeGen.End(); 
		} else SynErr(80);
	}

	void LastStatement(out Operand st) {
		st=null; 
		if (la.kind == 17) {
			Get();
			while (StartOf(3)) {
				ExprList(out st);
			}
			if (env.codeGen.IsScoped) { env.codeGen.Return(st); env.codeGen.End(); } 
		} else if (la.kind == 6) {
			Get();
			env.codeGen.Break(); 
			env.codeGen.End(); 
		} else SynErr(81);
	}

	void Block(out Operand st) {
		st = null; 
		env.OpenScope(); 
		Chunk(out st);
		env.CloseScope(); 
	}

	void FuncBlock(out Operand st) {
		st = null; 
		env.OpenScope(); 
		Chunk(out st);
		env.codeGen.EndFunc(); env.CloseScope(); 
	}

	void Expression(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		OrExpr(out lop);
		op=lop; 
		if (assgnOps[la.kind]) {
			AssignmentOperator(out oper);
			Expression(out rop);
			op=env.AssignOperandFromOperator(oper, lop, rop, env.codeGen.IsScoped); 
		}
	}

	void NameList(out Operand st) {
		var operands=new List<Operand>(); 
		Name(out st);
		operands.Add(st); 
		while (la.kind == 25) {
			Get();
			Name(out st);
			operands.Add(st); 
		}
		st = ListOfOperands.ToOperand(operands); 
	}

	void ExprList(out Operand st) {
		var operands=new List<Operand>(); 
		Expression(out st);
		operands.Add(st); 
		while (la.kind == 25) {
			Get();
			Expression(out st);
			operands.Add(st); 
		}
		st = new ListOfOperands(operands); 
	}

	void FuncName(out Operand st) {
		st=null; var name=""; 
		Expect(1);
		name=t.val; 
		while (la.kind == 28) {
			Get();
			Expect(1);
			name+=t.val; 
		}
		if (la.kind == 24) {
			Get();
			Expect(1);
			env.SetClassName(name); name=t.val; 
		}
		st=new Var(name); 
	}

	void FuncBody(Operand fn) {
		Operand st2; var args=new FuncArguments(); 
		Expect(36);
		if (la.kind == 1 || la.kind == 63) {
			ParList(out args);
		}
		Expect(52);
		env.codeGen.Emit(new FuncDef(fn, args)); 
		FuncBlock(out st2);
		Expect(59);
	}

	void Name(out Operand st) {
		st = null; 
		Expect(1);
		st=new Var(t.val); 
	}

	void Var(out Operand st) {
		st = null; 
		Name(out st);
		while (la.kind == 28) {
			Get();
			Name(out st);
		}
	}

	void ParList(out FuncArguments args) {
		List<string> names = new List<string>(); 
		if (la.kind == 1) {
			Get();
			names.Add(t.val); 
			while (la.kind == 25) {
				Get();
				Expect(1);
				names.Add(t.val); 
			}
			if (la.kind == 25) {
				Get();
				Expect(63);
			}
		} else if (la.kind == 63) {
			Get();
			names.Add("params dynamic[] args"); 
		} else SynErr(82);
		args=new FuncArguments(names); 
	}

	void FuncBodyLambda(out Operand st) {
		Operand st2; var args=new FuncArguments(); 
		Expect(36);
		if (la.kind == 1 || la.kind == 63) {
			ParList(out args);
		}
		Expect(52);
		st = new FuncDefLambda(args); 
		FuncBlock(out st2);
		Expect(59);
	}

	void TableConstructor(out Operand st) {
		st = null; 
		Expect(34);
		if (StartOf(4)) {
			FieldList(out st);
		}
		Expect(50);
		if (st==null) st=new ListOfOperandsCtor(); 
	}

	void FieldList(out Operand st) {
		Operand st1, st2; List<Operand> ops1=new List<Operand>(); List<Operand> ops2=new List<Operand>(); 
		Field(out st1, out st2);
		ops1.Add(st1); ops2.Add(st2); 
		while (la.kind == 25 || la.kind == 53) {
			FieldSep();
			Field(out st1, out st2);
			ops1.Add(st1); ops2.Add(st2); 
		}
		if (la.kind == 25 || la.kind == 53) {
			FieldSep();
		}
		st = new ListOfOperandsCtor(ops1, ops2); 
	}

	void Field(out Operand st, out Operand st2) {
		st = null; st2 = null; 
		if (la.kind == 35) {
			Get();
			Expression(out st);
			Expect(51);
			Expect(23);
			Expression(out st2);
		} else if (StartOf(3)) {
			Expression(out st);
		} else SynErr(83);
	}

	void FieldSep() {
		if (la.kind == 25) {
			Get();
		} else if (la.kind == 53) {
			Get();
		} else SynErr(84);
	}

	void Literal(out Operand op) {
		op=null; string memberName; 
		switch (la.kind) {
		case 1: {
			ClassOrIdent(out op, out memberName);
			break;
		}
		case 2: {
			Get();
			op=env.IntFromString(t.val); 
			break;
		}
		case 3: {
			Get();
			op=env.RealFromString(t.val); 
			break;
		}
		case 4: {
			Get();
			op=env.StringFromString(t.val); 
			break;
		}
		case 5: {
			Get();
			op=env.StringFromString(t.val); 
			break;
		}
		case 19: {
			Get();
			op=env.BoolFromString(t.val); 
			break;
		}
		case 10: {
			Get();
			op=env.BoolFromString(t.val); 
			break;
		}
		case 16: {
			Get();
			op=new Var("null"); 
			break;
		}
		default: SynErr(85); break;
		}
	}

	void ClassOrIdent(out Operand op, out String name ) {
		var args = new List<Operand>(); name=""; op=null; string fieldName=null; bool isSelf=false, isCall=false; 
		if (IsNotLPar()) {
			Expect(1);
			name = t.val; op=env.MemberFromString(ref name, ref isSelf); 
			while (la.kind == 24 || la.kind == 28) {
				if (la.kind == 28) {
					Get();
				} else {
					Get();
				}
				Expect(1);
				name += "." + t.val; if (!env.codeGen.IsScoped) name=t.val; 
				op=new Var(name); if (isSelf) { fieldName = t.val; isSelf=false; } 
			}
			if (la.kind == 36) {
				Get();
				if (StartOf(3)) {
					Argument(out op);
					args.Add(op); 
					while (la.kind == 25) {
						Get();
						Argument(out op);
						args.Add(op); 
					}
				}
				Expect(52);
				op=new Call(name, args); isCall=true; 
			}
			if (la.kind == 34) {
				TableConstructor(out op);
				args.Add(op); 
			}
		} else if (la.kind == 1) {
			Get();
			name=t.val; 
			Expect(36);
			if (StartOf(3)) {
				Argument(out op);
				args.Add(op); 
				while (la.kind == 25) {
					Get();
					Argument(out op);
					args.Add(op); 
				}
			}
			Expect(52);
			op=new Call(name, args); isCall=true; 
		} else SynErr(86);
		if (fieldName!=null && !isCall) env.AddField(t.val); 
	}

	void Argument(out Operand op) {
		Expression(out op);
	}

	void OrExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		AndExpr(out lop);
		op=lop; 
		while (la.kind == 64) {
			Get();
			oper=t.val; 
			AndExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void AssignmentOperator(out string oper) {
		oper=null; 
		Expect(23);
		oper=t.val; 
	}

	void AndExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		BitOrExpr(out lop);
		op=lop; 
		while (la.kind == 65) {
			Get();
			oper=t.val; 
			BitOrExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void BitOrExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		BitXorExpr(out lop);
		op=lop; 
		while (la.kind == 66) {
			Get();
			oper=t.val; 
			BitXorExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void BitXorExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		BitAndExpr(out lop);
		op=lop; 
		while (la.kind == 67) {
			Get();
			oper=t.val; 
			BitAndExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void BitAndExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		EqlExpr(out lop);
		op=lop; 
		while (la.kind == 21) {
			Get();
			oper=t.val; 
			EqlExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void EqlExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		RelExpr(out lop);
		op=lop; 
		while (la.kind == 30 || la.kind == 68) {
			if (la.kind == 68) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
			RelExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void RelExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		AddExpr(out lop);
		op=lop; 
		while (StartOf(5)) {
			if (la.kind == 38) {
				Get();
			} else if (la.kind == 31) {
				Get();
			} else if (la.kind == 39) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
			AddExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void AddExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		MulExpr(out lop);
		op=lop; 
		while (la.kind == 41 || la.kind == 47 || la.kind == 69) {
			if (la.kind == 47) {
				Get();
			} else if (la.kind == 41) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
			MulExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void MulExpr(out Operand op) {
		Operand lop, rop; op=null; string oper; 
		DotExpr(out lop);
		op=lop; 
		while (la.kind == 56 || la.kind == 70 || la.kind == 71) {
			if (la.kind == 56) {
				Get();
			} else if (la.kind == 70) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
			DotExpr(out rop);
			op=env.OperandFromOperator(oper, lop, rop); lop=op; 
		}
	}

	void DotExpr(out Operand op) {
		Operand lop, rop; string oper; op=null; 
		Unary(out lop);
		op=lop; 
		while (la.kind == 24 || la.kind == 28) {
			if (la.kind == 28) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
			Unary(out rop);
			op=env.OperandFromOperator(oper, lop, rop); 
		}
	}

	void Unary(out Operand op) {
		string oper=""; 
		while (la.kind == 41 || la.kind == 72 || la.kind == 73) {
			if (la.kind == 41) {
				Get();
			} else if (la.kind == 72) {
				Get();
			} else {
				Get();
			}
			oper=t.val; 
		}
		Primary(out op);
		op=env.OperandFromOperator(oper,op); 
	}

	void Primary(out Operand op) {
		List<Operand> args = new List<Operand>(); Operand op2=null; op=null; 
		if (StartOf(6)) {
			Literal(out op);
		} else if (la.kind == 36) {
			Get();
			Expression(out op);
			Expect(52);
		} else if (la.kind == 1) {
			Var(out op);
		} else if (la.kind == 34) {
			TableConstructor(out op);
		} else if (la.kind == 12) {
			Get();
			FuncBodyLambda(out op);
		} else SynErr(87);
		while (la.kind == 35) {
			Get();
			Expression(out op2);
			args.Add(op2); 
			Expect(51);
		}
		if (args.Any()) op=new ArrayAccess(op, args); 
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Lua();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,T,T, x,x,T,T, T,T,x,T, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,T,x,x},
		{x,T,T,T, T,T,x,T, x,x,T,T, T,T,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,T,x,x},
		{x,T,T,T, T,T,x,x, x,x,T,x, T,x,x,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x},
		{x,T,T,T, T,T,x,x, x,x,T,x, T,x,x,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, x,x,T,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "intCon expected"; break;
			case 3: s = "realCon expected"; break;
			case 4: s = "stringCon expected"; break;
			case 5: s = "stringCon2 expected"; break;
			case 6: s = "break expected"; break;
			case 7: s = "do expected"; break;
			case 8: s = "else expected"; break;
			case 9: s = "elseif expected"; break;
			case 10: s = "false expected"; break;
			case 11: s = "for expected"; break;
			case 12: s = "function expected"; break;
			case 13: s = "if expected"; break;
			case 14: s = "in expected"; break;
			case 15: s = "local expected"; break;
			case 16: s = "nil expected"; break;
			case 17: s = "return expected"; break;
			case 18: s = "repeat expected"; break;
			case 19: s = "true expected"; break;
			case 20: s = "while expected"; break;
			case 21: s = "and expected"; break;
			case 22: s = "andassgn expected"; break;
			case 23: s = "assgn expected"; break;
			case 24: s = "colon expected"; break;
			case 25: s = "comma expected"; break;
			case 26: s = "dec expected"; break;
			case 27: s = "divassgn expected"; break;
			case 28: s = "dot expected"; break;
			case 29: s = "dblcolon expected"; break;
			case 30: s = "eq expected"; break;
			case 31: s = "gt expected"; break;
			case 32: s = "gteq expected"; break;
			case 33: s = "inc expected"; break;
			case 34: s = "lbrace expected"; break;
			case 35: s = "lbrack expected"; break;
			case 36: s = "lpar expected"; break;
			case 37: s = "lshassgn expected"; break;
			case 38: s = "lt expected"; break;
			case 39: s = "lteq expected"; break;
			case 40: s = "ltlt expected"; break;
			case 41: s = "minus expected"; break;
			case 42: s = "minusassgn expected"; break;
			case 43: s = "modassgn expected"; break;
			case 44: s = "neq expected"; break;
			case 45: s = "not expected"; break;
			case 46: s = "orassgn expected"; break;
			case 47: s = "plus expected"; break;
			case 48: s = "plusassgn expected"; break;
			case 49: s = "question expected"; break;
			case 50: s = "rbrace expected"; break;
			case 51: s = "rbrack expected"; break;
			case 52: s = "rpar expected"; break;
			case 53: s = "scolon expected"; break;
			case 54: s = "tilde expected"; break;
			case 55: s = "tilde2 expected"; break;
			case 56: s = "times expected"; break;
			case 57: s = "timesassgn expected"; break;
			case 58: s = "xorassgn expected"; break;
			case 59: s = "\"end\" expected"; break;
			case 60: s = "\"until\" expected"; break;
			case 61: s = "\"then\" expected"; break;
			case 62: s = "\"require\" expected"; break;
			case 63: s = "\"...\" expected"; break;
			case 64: s = "\"or\" expected"; break;
			case 65: s = "\"and\" expected"; break;
			case 66: s = "\"|\" expected"; break;
			case 67: s = "\"^\" expected"; break;
			case 68: s = "\"~=\" expected"; break;
			case 69: s = "\"..\" expected"; break;
			case 70: s = "\"/\" expected"; break;
			case 71: s = "\"%\" expected"; break;
			case 72: s = "\"not\" expected"; break;
			case 73: s = "\"#\" expected"; break;
			case 74: s = "??? expected"; break;
			case 75: s = "invalid Statement"; break;
			case 76: s = "invalid Statement"; break;
			case 77: s = "invalid Statement"; break;
			case 78: s = "invalid Statement"; break;
			case 79: s = "invalid Statement"; break;
			case 80: s = "invalid Statement"; break;
			case 81: s = "invalid LastStatement"; break;
			case 82: s = "invalid ParList"; break;
			case 83: s = "invalid Field"; break;
			case 84: s = "invalid FieldSep"; break;
			case 85: s = "invalid Literal"; break;
			case 86: s = "invalid ClassOrIdent"; break;
			case 87: s = "invalid Primary"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
