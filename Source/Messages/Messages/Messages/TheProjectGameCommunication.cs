namespace Xsd2 {
    using System;
    using System.Collections.Generic;
    
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class Data : PlayerMessage {
        
        private TaskField[] taskFieldsField;
        
        private GoalField[] goalFieldsField;
        
        private Piece[] piecesField;
        
        private Location playerLocationField;
        
        private bool gameFinishedField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public TaskField[] TaskFields {
            get {
                return this.taskFieldsField;
            }
            set {
                this.taskFieldsField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public GoalField[] GoalFields {
            get {
                return this.goalFieldsField;
            }
            set {
                this.goalFieldsField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Piece[] Pieces {
            get {
                return this.piecesField;
            }
            set {
                this.piecesField = value;
            }
        }
        
        /// <uwagi/>
        public Location PlayerLocation {
            get {
                return this.playerLocationField;
            }
            set {
                this.playerLocationField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool gameFinished {
            get {
                return this.gameFinishedField;
            }
            set {
                this.gameFinishedField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class TaskField : Field {
        
        private int distanceToPieceField;
        
        private ulong pieceIdField;
        
        private bool pieceIdFieldSpecified;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int distanceToPiece {
            get {
                return this.distanceToPieceField;
            }
            set {
                this.distanceToPieceField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong pieceId {
            get {
                return this.pieceIdField;
            }
            set {
                this.pieceIdField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pieceIdSpecified {
            get {
                return this.pieceIdFieldSpecified;
            }
            set {
                this.pieceIdFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GoalField))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskField))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public abstract partial class Field : Location {
        
        private System.DateTime timestampField;
        
        private ulong playerIdField;
        
        private bool playerIdFieldSpecified;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId {
            get {
                return this.playerIdField;
            }
            set {
                this.playerIdField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool playerIdSpecified {
            get {
                return this.playerIdFieldSpecified;
            }
            set {
                this.playerIdFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Field))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GoalField))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskField))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class Location {
        
        private uint xField;
        
        private uint yField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint x {
            get {
                return this.xField;
            }
            set {
                this.xField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint y {
            get {
                return this.yField;
            }
            set {
                this.yField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class GoalField : Field {
        
        private GoalFieldType typeField;
        
        private TeamColour teamField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public GoalFieldType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TeamColour team {
            get {
                return this.teamField;
            }
            set {
                this.teamField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public enum GoalFieldType {
        
        /// <uwagi/>
        unknown,
        
        /// <uwagi/>
        goal,
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlEnumAttribute("non-goal")]
        nongoal,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public enum TeamColour {
        
        /// <uwagi/>
        red,
        
        /// <uwagi/>
        blue,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class Piece {
        
        private ulong idField;
        
        private PieceType typeField;
        
        private System.DateTime timestampField;
        
        private ulong playerIdField;
        
        private bool playerIdFieldSpecified;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PieceType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId {
            get {
                return this.playerIdField;
            }
            set {
                this.playerIdField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool playerIdSpecified {
            get {
                return this.playerIdFieldSpecified;
            }
            set {
                this.playerIdFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public enum PieceType {
        
        /// <uwagi/>
        unknown,
        
        /// <uwagi/>
        sham,
        
        /// <uwagi/>
        normal,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class Player {
        
        private TeamColour teamField;
        
        private PlayerType typeField;
        
        private ulong idField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TeamColour team {
            get {
                return this.teamField;
            }
            set {
                this.teamField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PlayerType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public enum PlayerType {
        
        /// <uwagi/>
        leader,
        
        /// <uwagi/>
        member,
    }
    
    /// <uwagi/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BetweenPlayersMessage))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class PlayerMessage {
        
        private ulong playerIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId {
            get {
                return this.playerIdField;
            }
            set {
                this.playerIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class TestPiece : GameMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public abstract partial class GameMessage {
        
        private string playerGuidField;
        
        private ulong gameIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string playerGuid {
            get {
                return this.playerGuidField;
            }
            set {
                this.playerGuidField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId {
            get {
                return this.gameIdField;
            }
            set {
                this.gameIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class PlacePiece : GameMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class PickUpPiece : GameMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class Move : GameMessage {
        
        private MoveType directionField;
        
        private bool directionFieldSpecified;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public MoveType direction {
            get {
                return this.directionField;
            }
            set {
                this.directionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool directionSpecified {
            get {
                return this.directionFieldSpecified;
            }
            set {
                this.directionFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public enum MoveType {
        
        /// <uwagi/>
        up,
        
        /// <uwagi/>
        down,
        
        /// <uwagi/>
        left,
        
        /// <uwagi/>
        right,
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class Discover : GameMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class AuthorizeKnowledgeExchange : GameMessage {
        
        private ulong withPlayerIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong withPlayerId {
            get {
                return this.withPlayerIdField;
            }
            set {
                this.withPlayerIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class KnowledgeExchangeRequest : BetweenPlayersMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public abstract partial class BetweenPlayersMessage : PlayerMessage {
        
        private ulong senderPlayerIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong senderPlayerId {
            get {
                return this.senderPlayerIdField;
            }
            set {
                this.senderPlayerIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class AcceptExchangeRequest : BetweenPlayersMessage {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class RejectKnowledgeExchange : BetweenPlayersMessage {
        
        private bool permanentField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool permanent {
            get {
                return this.permanentField;
            }
            set {
                this.permanentField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class Game : PlayerMessage {
        
        private Player[] playersField;
        
        private GameBoard boardField;
        
        private Location playerLocationField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public Player[] Players {
            get {
                return this.playersField;
            }
            set {
                this.playersField = value;
            }
        }
        
        /// <uwagi/>
        public GameBoard Board {
            get {
                return this.boardField;
            }
            set {
                this.boardField = value;
            }
        }
        
        /// <uwagi/>
        public Location PlayerLocation {
            get {
                return this.playerLocationField;
            }
            set {
                this.playerLocationField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    public partial class GameBoard {
        
        private uint widthField;
        
        private uint tasksHeightField;
        
        private uint goalsHeightField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint tasksHeight {
            get {
                return this.tasksHeightField;
            }
            set {
                this.tasksHeightField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint goalsHeight {
            get {
                return this.goalsHeightField;
            }
            set {
                this.goalsHeightField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class RegisterGame {
        
        private GameInfo newGameInfoField;
        
        /// <uwagi/>
        public GameInfo NewGameInfo {
            get {
                return this.newGameInfoField;
            }
            set {
                this.newGameInfoField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=true)]
    public partial class GameInfo {
        
        private string gameNameField;
        
        private ulong redTeamPlayersField;
        
        private ulong blueTeamPlayersField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string gameName {
            get {
                return this.gameNameField;
            }
            set {
                this.gameNameField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong redTeamPlayers {
            get {
                return this.redTeamPlayersField;
            }
            set {
                this.redTeamPlayersField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong blueTeamPlayers {
            get {
                return this.blueTeamPlayersField;
            }
            set {
                this.blueTeamPlayersField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class ConfirmGameRegistration {
        
        private ulong gameIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId {
            get {
                return this.gameIdField;
            }
            set {
                this.gameIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class RejectGameRegistration {
        
        private string gameNameField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string gameName {
            get {
                return this.gameNameField;
            }
            set {
                this.gameNameField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class GameStarted {
        
        private ulong gameIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId {
            get {
                return this.gameIdField;
            }
            set {
                this.gameIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class GetGames {
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class RegisteredGames {
        
        private GameInfo[] gameInfoField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlElementAttribute("GameInfo")]
        public GameInfo[] GameInfo {
            get {
                return this.gameInfoField;
            }
            set {
                this.gameInfoField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class JoinGame {
        
        private string gameNameField;
        
        private TeamColour preferredTeamField;
        
        private PlayerType preferredRoleField;
        
        private ulong playerIdField;
        
        private bool playerIdFieldSpecified;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string gameName {
            get {
                return this.gameNameField;
            }
            set {
                this.gameNameField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TeamColour preferredTeam {
            get {
                return this.preferredTeamField;
            }
            set {
                this.preferredTeamField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PlayerType preferredRole {
            get {
                return this.preferredRoleField;
            }
            set {
                this.preferredRoleField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId {
            get {
                return this.playerIdField;
            }
            set {
                this.playerIdField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool playerIdSpecified {
            get {
                return this.playerIdFieldSpecified;
            }
            set {
                this.playerIdFieldSpecified = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class ConfirmJoiningGame : PlayerMessage {
        
        private Player playerDefinitionField;
        
        private ulong gameIdField;
        
        private string privateGuidField;
        
        /// <uwagi/>
        public Player PlayerDefinition {
            get {
                return this.playerDefinitionField;
            }
            set {
                this.playerDefinitionField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId {
            get {
                return this.gameIdField;
            }
            set {
                this.gameIdField = value;
            }
        }
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string privateGuid {
            get {
                return this.privateGuidField;
            }
            set {
                this.privateGuidField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class RejectJoiningGame : PlayerMessage {
        
        private string gameNameField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string gameName {
            get {
                return this.gameNameField;
            }
            set {
                this.gameNameField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class GameMasterDisconnected {
        
        private ulong gameIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId {
            get {
                return this.gameIdField;
            }
            set {
                this.gameIdField = value;
            }
        }
    }
    
    /// <uwagi/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
    public partial class PlayerDisconnected {
        
        private ulong playerIdField;
        
        /// <uwagi/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId {
            get {
                return this.playerIdField;
            }
            set {
                this.playerIdField = value;
            }
        }
    }
}
