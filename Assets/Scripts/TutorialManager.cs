using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class TutorialManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		[SerializeField] private UIManager uiManager;
		[SerializeField] private BlockPlacerManager blockPlacerManager;
		[SerializeField] private GameObject stockAHighlight;
		[SerializeField] private GameObject stock1Highlight;
		[SerializeField] private GameObject outputHighlight;
		[SerializeField] private GameObject p1ControlHighlight;
		[SerializeField] private GameObject p2ControlHighlight;
		[SerializeField] private GameObject goalHighlight;
		[SerializeField] private GameObject controlTutorialBlock;
		[SerializeField] private TextFlowRunner introTutorialTextFlow;
		[SerializeField] private TextFlowRunner controlTutorialTextFlow;
		[SerializeField] private TextFlowRunner planModeTutorialTextFlow;
		[SerializeField] private TextFlowRunner mergeBlockTutorialTextFlow;
		[SerializeField] private TextFlowRunner loopBlockTutorialTextFlow;
		[SerializeField] private TextFlowRunner switchBlockTutorialTextFlow;

		private TutorialStep step;
		private TutorialGoalKeeper goalKeeper;
		private List<PipelinePathMono> paths;

		public enum TutorialStep
		{
			Undefined,
			Intro,
			Control,
			PlanMode,
			MergeBlock,
			LoopBlock,
			SwitchBlock,
			End,
		}

		private void Start()
		{
			StartStep();
		}

		public void SetTutorialMode(TutorialGoalKeeper goalKeeper)
		{
			step = GameSessionManager.Instance.TutorialStep;
			paths = new List<PipelinePathMono>();

			this.goalKeeper = goalKeeper;
			this.goalKeeper.OnScoreAdded += HandleScoreAdded;
			this.goalKeeper.OnAllGoalsComplete += HandleAllGoalsComplete;

			gameManager.PlanModeTrigger += HandlePlanModeTrigger;
			blockPlacerManager.BlockPlaced += HandleBlockPlaced;
			blockPlacerManager.ConnectionCreated += HandleConnectionCreated;
		}

		public void OnTextFlowStep(TextFlowRunner textFlow, int index, int count)
		{
			switch (step)
			{
				case TutorialStep.Intro:
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.Control:
					switch (index)
					{
						case 0:
							goalKeeper.GetNewGoals();
							goalHighlight.SetActive(true);
							break;
						case 2:
							goalHighlight.SetActive(false);
							stock1Highlight.SetActive(true);
							stockAHighlight.SetActive(true);
							outputHighlight.SetActive(true);
							break;
						case 3:
							stock1Highlight.SetActive(false);
							stockAHighlight.SetActive(false);
							outputHighlight.SetActive(false);
							p1ControlHighlight.SetActive(true);
							p2ControlHighlight.SetActive(true);
							break;
						case 4:
							p1ControlHighlight.SetActive(false);
							p2ControlHighlight.SetActive(false);
							controlTutorialBlock.SetActive(false);
							stock1Highlight.SetActive(true);
							stockAHighlight.SetActive(true);
							outputHighlight.SetActive(true);
							textFlow.SetNextButtonState(false);
							break;
						case 6:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.PlanMode:
					switch (index)
					{
						case 1:
							goalKeeper.GetNewGoals();
							break;
						case 3:
							uiManager.SetPlanModeButton(true, true);
							textFlow.SetNextButtonState(false);
							break;
						case 4:
							uiManager.SetPlanModeButton(true, false);
							textFlow.SetNextButtonState(true);
							break;
						case 5:
							uiManager.SetBlockIconLockState(false, 0);
							textFlow.SetNextButtonState(false);
							break;
						case 6:
							uiManager.SetBlockIconLockState(false, 1);
							break;
						case 8:
							textFlow.SetNextButtonState(true);
							break;
						case 9:
							uiManager.SetPlanModeButton(true, true);
							textFlow.SetNextButtonState(false);
							break;
						case 11:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.MergeBlock:
					switch (index)
					{
						case 0:
							goalKeeper.GetNewGoals();
							break;
						case 1:
							textFlow.SetNextButtonState(false);
							uiManager.SetBlockIconLockState(false, 0);
							uiManager.SetBlockIconLockState(false, 1);
							uiManager.SetBlockIconLockState(false, 2);
							break;
						case 3:
							textFlow.SetNextButtonState(true);
							break;
						case 4:
							textFlow.SetNextButtonState(false);
							break;
						case 5:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.LoopBlock:
					switch (index)
					{
						case 1:
							goalKeeper.GetNewGoals();
							break;
						case 3:
							uiManager.SetBlockIconLockState(false, 0);
							uiManager.SetBlockIconLockState(false, 1);
							uiManager.SetBlockIconLockState(false, 2);
							uiManager.SetBlockIconLockState(false, 3);
							textFlow.SetNextButtonState(false);
							break;
						case 5:
							textFlow.SetNextButtonState(true);
							break;
						case 6:
							textFlow.SetNextButtonState(false);
							break;
						case 7:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
				case TutorialStep.SwitchBlock:
					switch (index)
					{
						case 2:
							textFlow.SetNextButtonState(false);
							uiManager.SetBlockIconLockState(false);
							break;
						case 3:
							textFlow.SetNextButtonState(true);
							break;
						case 5:
							goalKeeper.GetNewGoals();
							textFlow.SetNextButtonState(false);
							break;
						case 6:
							textFlow.SetNextButtonState(true);
							break;
					}
					if (index >= count)
					{
						EndStep();
					}
					break;
			}
		}

		public void StartStep()
		{
			switch (step)
			{
				case TutorialStep.Intro:
					introTutorialTextFlow.gameObject.SetActive(true);
					controlTutorialBlock.SetActive(true);
					uiManager.SetPlanModeButton(false, false);
					uiManager.SetBlockIconLockState(true);
					break;
				case TutorialStep.Control:
					controlTutorialTextFlow.gameObject.SetActive(true);
					goalKeeper.SetGeneratorStep(step);
					uiManager.SetPlanModeButton(false, false);
					uiManager.SetBlockIconLockState(true);
					break;
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.gameObject.SetActive(true);
					uiManager.SetPlanModeButton(false, false);
					uiManager.SetBlockIconLockState(true);
					break;
				case TutorialStep.MergeBlock:
					goalKeeper.SetGeneratorStep(TutorialStep.MergeBlock);
					mergeBlockTutorialTextFlow.gameObject.SetActive(true);
					uiManager.SetBlockIconLockState(true);
					break;
				case TutorialStep.LoopBlock:
					goalKeeper.SetGeneratorStep(TutorialStep.LoopBlock);
					loopBlockTutorialTextFlow.gameObject.SetActive(true);
					uiManager.SetBlockIconLockState(true);
					break;
				case TutorialStep.SwitchBlock:
					goalKeeper.SetGeneratorStep(TutorialStep.SwitchBlock);
					switchBlockTutorialTextFlow.gameObject.SetActive(true);
					uiManager.SetBlockIconLockState(true);
					break;
			}
		}

		public void EndStep()
		{
			switch (step)
			{
				case TutorialStep.Intro:
					introTutorialTextFlow.gameObject.SetActive(false);
					break;
				case TutorialStep.Control:
					stock1Highlight.SetActive(false);
					stockAHighlight.SetActive(false);
					outputHighlight.SetActive(false);
					controlTutorialTextFlow.gameObject.SetActive(false);
					break;
				case TutorialStep.PlanMode:
					planModeTutorialTextFlow.gameObject.SetActive(false);
					break;
				case TutorialStep.MergeBlock:
					mergeBlockTutorialTextFlow.gameObject.SetActive(false);
					GameSessionManager.Instance.SaveClearedTutorial(0);
					break;
				case TutorialStep.LoopBlock:
					loopBlockTutorialTextFlow.gameObject.SetActive(false);
					break;
				case TutorialStep.SwitchBlock:
					switchBlockTutorialTextFlow.gameObject.SetActive(false);
					GameSessionManager.Instance.SaveClearedTutorial(1);
					GameSessionManager.Instance.ReturnToMainMenu();
					break;
			}
			step++;
			StartStep();
		}

		private void HandleScoreAdded(int addedScore)
		{
			switch (step)
			{
				case TutorialStep.Control:
					if (addedScore < 0)
					{
						controlTutorialTextFlow.Next();
					}
					break;
			}
		}

		private void HandleAllGoalsComplete()
		{
			switch (step)
			{
				case TutorialStep.Control:
					if (controlTutorialTextFlow.CurrentIndex == 4 || controlTutorialTextFlow.CurrentIndex == 5)
					{
						controlTutorialTextFlow.SetStep(6, true);
					}
					break;
				case TutorialStep.PlanMode:
					if (planModeTutorialTextFlow.CurrentIndex == 10)
					{
						planModeTutorialTextFlow.SetStep(11, true);
					}
					break;
				case TutorialStep.MergeBlock:
					if (mergeBlockTutorialTextFlow.CurrentIndex == 4)
					{
						mergeBlockTutorialTextFlow.SetStep(5, true);
					}
					break;
				case TutorialStep.LoopBlock:
					if (loopBlockTutorialTextFlow.CurrentIndex == 6)
					{
						loopBlockTutorialTextFlow.SetStep(7, true);
					}
					break;
				case TutorialStep.SwitchBlock:
					if (switchBlockTutorialTextFlow.CurrentIndex == 5)
					{
						switchBlockTutorialTextFlow.SetStep(6, true);
					}
					break;
			}
		}

		private void HandlePlanModeTrigger(bool isIn)
		{
			switch (step)
			{
				case TutorialStep.PlanMode:
					if (isIn)
					{
						planModeTutorialTextFlow.SetStep(4, true);
					}
					else
					{
						planModeTutorialTextFlow.SetStep(10, true);
					}
					break;
			}
		}

		private void HandleBlockPlaced(BlockMono block)
		{
			int startBlockCount = blockPlacerManager.GetBlockCount(typeof(StartBlockMono));
			int upBlockCount = blockPlacerManager.GetBlockCount(typeof(UpgradeBlockMono));
			int mergeBlockCount = blockPlacerManager.GetBlockCount(typeof(MergeBlockMono));
			int loopBlockCount = blockPlacerManager.GetBlockCount(typeof(LoopBlockMono));
			int switchBlockCount = blockPlacerManager.GetBlockCount(typeof(ConditionBlockMono));
			switch (step)
			{
				case TutorialStep.PlanMode:
					if (planModeTutorialTextFlow.CurrentIndex == 5 &&
						startBlockCount > 0)
					{
						planModeTutorialTextFlow.SetStep(6, true);
					}
					else if (planModeTutorialTextFlow.CurrentIndex == 6 &&
						upBlockCount > 0)
					{
						planModeTutorialTextFlow.SetStep(7, true);
					}
					break;
				case TutorialStep.MergeBlock:
					if (mergeBlockTutorialTextFlow.CurrentIndex == 1 &&
						startBlockCount >= 2 &&
						mergeBlockCount > 0)
					{
						mergeBlockTutorialTextFlow.SetStep(2, true);
					}
					break;
				case TutorialStep.LoopBlock:
					if (loopBlockTutorialTextFlow.CurrentIndex == 3 &&
						startBlockCount > 0 &&
						upBlockCount > 0 &&
						loopBlockCount > 0)
					{
						loopBlockTutorialTextFlow.SetStep(4, true);
					}
					break;
			}
		}

		private void HandleConnectionCreated(PipelinePathMono pathMono)
		{
			paths.Add(pathMono);
			switch (step)
			{
				case TutorialStep.PlanMode:
					if (pathMono.StartGate.Block is StartBlockMono && pathMono.EndGate.Block is UpgradeBlockMono)
					{
						planModeTutorialTextFlow.SetStep(8, true);
					}
					break;
				case TutorialStep.MergeBlock:
					if (pathMono.EndGate.Block is MergeBlockMono)
					{
						for (int i = paths.Count - 2; i >= 0; i--) // since pathMono is the last one
						{
							if (paths[i] == null)
							{
								paths.RemoveAt(i); // Clear space for a bit
								continue;
							}
							PipelinePathMono anotherMergePath = paths[i];
							if (anotherMergePath.EndGate.Block == pathMono.EndGate.Block)
							{
								mergeBlockTutorialTextFlow.SetStep(3, true);
								break;
							}
						}
					}
					break;
				case TutorialStep.LoopBlock:
					for (int i = paths.Count - 1; i >= 0; i--)
					{
						if (paths[i] == null)
						{
							paths.RemoveAt(i); // Clear space for a bit
							continue;
						}
						if (paths[i].StartGate.Block is StartBlockMono)
						{
							if ((paths[i].EndGate.Block is LoopBlockMono loopBlock) &&
								loopBlock.LoopOutGate.GetConnectedPath() != null &&
								loopBlock.LoopInGate.GetConnectedPath() != null &&
								loopBlock.LoopOutGate.GetConnectedPath().EndGate.Block is UpgradeBlockMono &&
								loopBlock.LoopOutGate.GetConnectedPath().EndGate.Block == loopBlock.LoopInGate.GetConnectedPath().StartGate.Block)
							{
								loopBlockTutorialTextFlow.SetStep(5, true);
								break;
							}
						}
					}
					break;
				case TutorialStep.SwitchBlock:
					if (switchBlockTutorialTextFlow.CurrentIndex == 2 && pathMono.StartGate.Block is StartBlockMono && pathMono.EndGate.Block is ConditionBlockMono)
					{
						switchBlockTutorialTextFlow.SetStep(3, true);
					}
					break;
			}
		}
	}
}